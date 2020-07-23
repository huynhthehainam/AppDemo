using AMG.App.API.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AMG.App.API.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            ActionResponse actionResponse = new ActionResponse();
            actionResponse.Data = new { CreatedBy = "Nam" };
            return actionResponse.ToIActionResult();
        }
    }
}