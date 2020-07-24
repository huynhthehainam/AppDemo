using Microsoft.AspNetCore.Mvc.Filters;

namespace AMG.App.API.Permissions
{
    public class IsProjectOwner : IPermission
    {
        public bool HasPermission(ActionExecutingContext context)
        {
            return false;
        }
    }
}