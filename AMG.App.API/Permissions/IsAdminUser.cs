using System;
using AMG.App.API.Models.Responses;
using AMG.App.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AMG.App.API.Permissions
{
    public class IsAdminUser : IPermission
    {
        public IsAdminUser()
        {

        }
        public bool HasPermission(ActionExecutingContext context)
        {
            var currentUser = new UserCache(context.HttpContext.User);
            if (currentUser != null)
            {
                return currentUser.IsAdmin;
            }
            return false;
        }
    }
}