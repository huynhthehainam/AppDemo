using System;
using AMG.App.API.Models.Commands;
using AMG.App.API.Models.Responses;
using AMG.App.API.Services;
using AMG.App.DAL.Models;
using AMG.App.DAL.Services;
using AMG.App.Infrastructure.Constants;
using AMG.App.Infrastructure.Helpers;
using AMG.App.Infrastructure.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace AMG.App.API.Controllers
{
    public class AuthController : BaseController
    {
        private JWTService jWTService;
        private AuthSettings authSettings;
        private AuthService authService;
        public AuthController(JWTService jWTService, UserService userService, IOptions<AuthSettings> authSettings, AuthService authService) : base(userService)
        {
            this.jWTService = jWTService;
            this.authSettings = authSettings.Value;
            this.authService = authService;
        }
        private LoginResponse ProcessLogin(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(ExpiredTime.AccessTokenExpirationTime);
            var accessToken = jWTService.GenerateAccessToken(authSettings.AuthSecret, user, accessTokenExpiration);
            var refreshToken = TokenHelper.GenerateToken();
            // authService.RemoveAllRefreshToken(requestIP);
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(ExpiredTime.AccessTokenExpirationTime)
            };
            authService.SaveUserCache(user, cacheEntryOptions);
            // authService.SaveAuthRefreshToken(user, refreshToken, requestIP);
            return new LoginResponse { UserId = user.Id, AccessToken = accessToken, RefreshToken = refreshToken };
        }
        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody] LoginCommand command)
        {
            ActionResponse actionResponse = new ActionResponse();
            var user = userService.GetUser(command.Email);
            if (HashHelper.Verify(command.Password, user.Password))
            {
                actionResponse.Data = ProcessLogin(user);
            }
            else
            {
                actionResponse.AddInvalidErr("Username or password");
            }

            return actionResponse.ToIActionResult();
        }
    }
}