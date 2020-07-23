using AMG.App.DAL.Services;
using Microsoft.AspNetCore.Mvc;

namespace AMG.App.API.Controllers
{
    [Route("[controller]")]
    public class BaseController : Controller
    {
        protected UserService userService;
        public BaseController(UserService userService)
        {
            this.userService = userService;
        }

    }
}