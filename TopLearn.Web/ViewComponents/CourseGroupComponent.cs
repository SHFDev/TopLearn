using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.ViewComponents
{
    public class CourseGroupComponent : ViewComponent
    {
        ICourseService _coursesService;
        public CourseGroupComponent(ICourseService coursesService)
        {
            _coursesService = coursesService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
             return await Task.FromResult((IViewComponentResult)View("CourseGroup" ,_coursesService.GetAllGroup()));  
        }




    }
}
