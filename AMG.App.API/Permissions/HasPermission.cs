using System;
using System.Collections.Generic;
using System.Linq;
using AMG.App.API.Models.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AMG.App.API.Permissions
{

    public interface IPermission
    {
        bool HasPermission(ActionExecutingContext context);
    }
    public class HasPermission : ActionFilterAttribute
    {
        public List<IPermission> permissions = new List<IPermission>();
        public HasPermission(params Type[] permissionTypes)
        {
            this.permissions = GetPermission(permissionTypes);
        }
        private List<IPermission> GetPermission(Type[] permissionTypes)
        {
            List<IPermission> permissions = new List<IPermission>();
            foreach (var permissionType in permissionTypes)
            {
                if (typeof(IPermission).IsAssignableFrom(permissionType))
                {
                    permissions.Add((IPermission)Activator.CreateInstance(permissionType));
                }
                else
                {
                    throw new InvalidCastException($"{permissionType.ToString()} is not a IPermission");
                }
            }
            return permissions;
        }
        public HasPermission() { }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ActionResponse actionResponse = new ActionResponse();
            if (permissions.Any(ww => !ww.HasPermission(context)))
            {
                actionResponse.AddNotAllowedErr();
                context.Result = actionResponse.ToIActionResult();
            }
        }
    }
}