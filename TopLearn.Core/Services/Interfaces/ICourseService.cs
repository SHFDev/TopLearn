using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.Core.DTOs.Course;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> GetAllGroup();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatues();

        #endregion


        #region Courses
        List<ShowCourseForAdminViewModel> GetCorsesForAdmin(); 
        int AddCourse(Course course ,IFormFile imgCourse ,IFormFile CourseDemo);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile demo);
        #endregion



        #region Episode
         int AddEpisode(CourseEpisode episode,IFormFile fileEpisode);
        bool CheckExistFile(string fileName);
        List<CourseEpisode> GetListEpisodeCourse(int  courseId);

        CourseEpisode GetEpisodeById(int episodeId);
        void EditEpisode(CourseEpisode episode, IFormFile episodeFile);
        #endregion

    }
}
