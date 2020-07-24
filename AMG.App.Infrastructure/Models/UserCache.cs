using System.Linq;
using System;
using System.Security.Claims;
namespace AMG.App.Infrastructure.Models
{
    public class UserCache
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; } = false;
        public UserCache() { }
        public UserCache(long id, string email, bool isAdmin)
        {
            this.Id = id;
            this.Email = email;
            this.IsAdmin = isAdmin;
        }
        public UserCache(ClaimsPrincipal user)
        {
            Id = Convert.ToInt64(user.Claims.FirstOrDefault(ww => ww.Type == "Id")?.Value);
            Email = Convert.ToString(user.Claims.FirstOrDefault(ww => ww.Type == "Email")?.Value);
            IsAdmin = Convert.ToBoolean(user.Claims.FirstOrDefault(ww => ww.Type == "IsAdmin")?.Value);
        }
    }
}