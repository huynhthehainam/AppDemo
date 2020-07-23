using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AMG.App.DAL.Services;
using AMG.App.Infrastructure.Constants;
using AMG.App.Infrastructure.Models;
using Microsoft.AspNetCore.Http;

namespace AMG.App.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;
        public AuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, JWTService jWTService)
        {
            string authHeader = context.Request.Headers[Key.AuthHeaderKey];
            Console.WriteLine($"Auth: {authHeader}");
            if (authHeader != null)
            {
                authHeader = authHeader.Replace(Key.JWTPrefixKey, "").Trim();
                UserCache user = jWTService.GetUserCache(authHeader);
                if (user == null)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
                ClaimsIdentity aa = new ClaimsIdentity();
                var claims = new[]{
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                };
                var identity = new ClaimsIdentity(claims, "basic");
                context.User = new ClaimsPrincipal(identity);
            }
            await next(context);

        }
    }
}