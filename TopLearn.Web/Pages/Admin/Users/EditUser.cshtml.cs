using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class EditUser : PageModel
    {
        private IUserService _userService;
        private IPermissionService _permission;

        public EditUser(IUserService userService, IPermissionService permission)
        {
            _userService = userService;
            _permission = permission;
        }

        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }


        public void OnGet(int id)
        {
            EditUserViewModel = _userService.GetUserForShowInEditMode(id);
            ViewData["Roles"] = _permission.GetRoles();
        }


        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _userService.EditUserUserFromAdmin(EditUserViewModel);
            _permission.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
            return RedirectToPage("Index");
            //UserForAdminViewModel = _userService.GetUsers(pageId,filterEmail,filterUserName);
        }

    }
}