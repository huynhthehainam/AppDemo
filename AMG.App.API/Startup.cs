using AMG.App.DAL.Databases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AMG.App.Infrastructure.Constants;
using AMG.App.DAL.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using AMG.App.API.Middlewares;
using Microsoft.AspNetCore.HttpOverrides;
using AMG.App.Infrastructure.Models.Settings;
using AMG.App.API.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace AMG.App.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEntityFrameworkNpgsql().AddDbContext<DatabaseContext>((sp, opt) => opt.UseNpgsql(Configuration.GetConnectionString(DbConnection.DatabaseKey), b =>
            {
                b.MigrationsAssembly("AMG.App.API");
            }).UseInternalServiceProvider(sp));
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = Configuration["RedisSettings:Host"] + ":" + Configuration["RedisSettings:Port"] + ",connectTimeout=10000,syncTimeout=10000";
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            });



            services.AddScoped<UserService, UserService>();
            services.AddScoped<AuthService, AuthService>();
            services.AddScoped<JWTService, JWTService>();


            var authSettingsSection = Configuration.GetSection("AuthSettings");
            services.Configure<AuthSettings>(authSettingsSection);
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            var appSettings = authSettingsSection.Get<AuthSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.AuthSecret);
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddMvcCore().AddAuthorization();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (!Directory.Exists(FolderPath.StaticFilePath))
            {
                Directory.CreateDirectory(FolderPath.StaticFilePath);
            }
            if (!Directory.Exists(Path.Combine(FolderPath.StaticFilePath, FolderPath.ImagePath)))
            {
                Directory.CreateDirectory(Path.Combine(FolderPath.StaticFilePath, FolderPath.ImagePath));
            }
            if (!Directory.Exists(Path.Combine(FolderPath.StaticFilePath, FolderPath.TemplatePath)))
            {
                Directory.CreateDirectory(Path.Combine(FolderPath.StaticFilePath, FolderPath.TemplatePath));
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(FolderPath.StaticFilePath),
                RequestPath = new PathString("/staticfiles"),
            });

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<AuthMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
