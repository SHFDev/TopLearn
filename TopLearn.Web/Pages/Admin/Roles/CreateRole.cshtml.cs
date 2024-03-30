using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security;
using TopLearn.Core.Services;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    public class CreateRoleModel : PageModel
    {
        IPermissionService _permissionService;
        public CreateRoleModel(IPermissionService permission)
        {
            _permissionService = permission;
        }


        [BindProperty]
        public Role  Role { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) 
            {
                return Page();
            }
            Role.IsDelete = false;
            int roleId=_permissionService.AddRole(Role);
            //ToDO add permission

            return RedirectToPage("Index");
        }
    }
}