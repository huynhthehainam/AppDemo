using AMG.App.DAL.Services;
using AMG.App.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace AMG.App.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BaseController : Controller
    {
        protected UserService userService;
        public BaseController(UserService userService)
        {
            this.userService = userService;
        }
        public UserCache CurrentUser
        {
            get
            {
                return new UserCache(HttpContext.User);
            }
        }
    }
}