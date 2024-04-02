using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Core.Security
{
    public class PermissioncheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        IPermissionService _permissionService;
        int _permissionId = 0;
        public PermissioncheckerAttribute(int permissionId)
        {
            _permissionId = permissionId; 
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
          _permissionService=(IPermissionService)context.HttpContext.RequestServices.GetService(typeof(IPermissionService));
            
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string username = context.HttpContext.User.Identity.Name;

                if (!_permissionService.CheckPermission(_permissionId, username))
                {
                    context.Result = new RedirectResult("/Login");
                }

            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }
        }
    }
}
