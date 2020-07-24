using System;
using AMG.App.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AMG.App.API.Permissions
{
    public class IsVIPUser : IPermission
    {
        public bool HasPermission(ActionExecutingContext context)
        {
            var currentUser = new UserCache(context.HttpContext.User);
            if (currentUser != null)
            {
                return true;
            }
            return false;
        }
    }
}