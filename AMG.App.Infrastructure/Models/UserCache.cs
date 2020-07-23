using System.Linq;
using System;
using System.Security.Claims;
namespace AMG.App.Infrastructure.Models
{
    public class UserCache
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public UserCache() { }
        public UserCache(long id, string email)
        {
            this.Id = id;
            this.Email = email;
        }
        public UserCache(ClaimsPrincipal user)
        {
            Id = Convert.ToInt64(user.Claims.FirstOrDefault(ww => ww.Type == "Id").Value);
            Email = Convert.ToString(user.Claims.FirstOrDefault(ww => ww.Type == "Email").Value);
        }
    }
}