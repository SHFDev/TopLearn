using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;
using System.Security;
using TopLearn.Core.Security;
using TopLearn.Core.Services;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [Permissionchecker(7)]
    public class CreateRoleModel : PageModel
    {
        IPermissionService _permissionService;
        public CreateRoleModel(IPermissionService permission)
        {
            _permissionService = permission;
        }


        [BindProperty]
        public Role Role { get; set; }
        public void OnGet()
        {
            ViewData["Permissions"] = _permissionService.GetAllPermission();


        }

        public IActionResult OnPost(List<int> selectedpermission)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Role.IsDelete = false;
            int roleId = _permissionService.AddRole(Role);

            _permissionService.AddPermissionsToRole(roleId, selectedpermission);
            //ToDO add permission

            return RedirectToPage("Index");
        }
    }
}