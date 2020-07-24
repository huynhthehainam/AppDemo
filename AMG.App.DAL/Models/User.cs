using AMG.App.Infrastructure.Models.Data;

namespace AMG.App.DAL.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
    }
}