using System.Linq;
using AMG.App.DAL.Databases;
using AMG.App.DAL.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace AMG.App.DAL.Services
{
    public class UserService : BaseService
    {
        public UserService(DatabaseContext context, IDistributedCache distributedCache) : base(context, distributedCache)
        {

        }
        public User GetUser(string email)
        {
            return context.Users.FirstOrDefault(ww => ww.Email == email);
        }
    }
}