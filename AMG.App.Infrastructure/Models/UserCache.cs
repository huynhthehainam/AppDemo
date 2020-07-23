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
    }
}