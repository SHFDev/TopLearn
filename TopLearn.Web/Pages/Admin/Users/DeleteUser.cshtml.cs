using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    public class DeleteUserModel : PageModel
    {
        IUserService _userService;

        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }
       public InformationUserViewModel InformationUserViewModel { get; set; }

        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            InformationUserViewModel = _userService.GetUserInformation(id);  
        }
        public  IActionResult OnPost(int Userid)
        {
            _userService.DeleteUser(Userid);
            return RedirectToPage("Index");
        }
    }
}
    