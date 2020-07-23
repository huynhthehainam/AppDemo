using AMG.App.DAL.Databases;
using AMG.App.DAL.Models;
using AMG.App.Infrastructure.Models;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using System;
using AMG.App.Infrastructure.Constants;

namespace AMG.App.DAL.Services
{
    public class AuthService : BaseService
    {
        public AuthService(DatabaseContext context, IDistributedCache distributedCache) : base(context, distributedCache)
        {

        }
        public void SaveUserCache(User user, DistributedCacheEntryOptions cacheEntryOptions)
        {
            string authUserKey = $"{Key.AuthCacheKey}:{user.Id}";
            distributedCache.Remove(authUserKey);
            UserCache userCache = new UserCache(user.Id, user.Email);
            distributedCache.SetString(authUserKey, JsonSerializer.Serialize(userCache), cacheEntryOptions);
        }
    }
}