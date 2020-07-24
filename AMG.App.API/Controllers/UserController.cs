using System.Collections.Generic;
using AMG.App.API.Models.Responses;
using AMG.App.API.Permissions;
using AMG.App.DAL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMG.App.API.Controllers
{
    public class UserController : BaseController
    {
        public UserController(UserService userService) : base(userService)
        {

        }
        [Route("profile")]
        [HttpGet]
        [Authorize]
        [HasPermission(typeof(IsAdminUser), typeof(IsVIPUser))]
        public IActionResult Profile()
        {
            ActionResponse actionResponse = new ActionResponse();
            actionResponse.Data = CurrentUser;
            return actionResponse.ToIActionResult();
        }
    }
}