using AMG.App.DAL.Models;
using AMG.App.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AMG.App.DAL.Databases
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(ww =>
            {
                ww.HasIndex(ww => ww.Email).IsUnique();
            });



            //Seeding

            modelBuilder.Entity<User>().HasData(new User { Email = "mail@mail.com", Password = HashHelper.Hash("123"), Id = 1 });
        }
    }
}