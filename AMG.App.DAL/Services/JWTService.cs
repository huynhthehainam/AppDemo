using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using AMG.App.DAL.Databases;
using AMG.App.Infrastructure.Models;
using AMG.App.Infrastructure.Constants;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using AMG.App.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace AMG.App.DAL.Services
{
    public class JWTService : BaseService
    {
        public JWTService(DatabaseContext context, IDistributedCache distributedCache) : base(context, distributedCache)
        {

        }
        public UserCache GetUserCache(string token)
        {
            UserCache user = null;
            var validator = new JwtSecurityTokenHandler();
            var jwtToken = validator.ReadJwtToken(token);
            var userClaim = jwtToken.Claims.FirstOrDefault(ww => ww.Type == "userId");
            if (userClaim != null)
            {
                string hashCode = userClaim.Value;
                var hashIds = new HashidsNet.Hashids(salt: Salt.IdSalt);
                var data = hashIds.DecodeLong(hashCode);
                long userId = Convert.ToInt64(data[1]);
                user = GetUserCache(userId);
            }
            return user;
        }

        public string GenerateAccessToken(string authSecret, User user, DateTime accessTokenExpiration)
        {
            DateTimeOffset dto = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);
            var unixDate = dto.ToUnixTimeMilliseconds();
            var hashIds = new HashidsNet.Hashids(salt: Salt.IdSalt);
            var userCode = hashIds.EncodeLong(unixDate, user.Id);

            var claims = new[] {
                new Claim("userId", userCode),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(claims: claims,
                                              expires: accessTokenExpiration,
                                              signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserCache GetUserCache(long userId)
        {
            UserCache user = null;
            string userKey = $"{Key.AuthCacheKey}:{userId}";
            var userJson = this.distributedCache.GetString(userKey);
            if (!string.IsNullOrEmpty(userJson))
            {
                user = JsonSerializer.Deserialize<UserCache>(userJson);
            }
            return user;
        }
    }
}