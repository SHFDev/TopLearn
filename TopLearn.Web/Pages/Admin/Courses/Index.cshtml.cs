using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Services;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Courses
{
    public class IndexModel : PageModel
    {
        ICourseService _courseService;
        public IndexModel(ICourseService courseService)
        {
                _courseService = courseService;
        }
        public List<ShowCourseForAdminViewModel> ListCourse { get; set; }

        public void OnGet()     
        {
            ListCourse= _courseService.GetCorsesForAdmin();
        }
    }   
}
