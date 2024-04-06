using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Generator;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services
{
    public class CourseService : ICourseService
    {
        TopLearnContext _context;
        public CourseService(TopLearnContext context)
        {
            _context = context;
        }

        public int AddCourse(Course course, IFormFile imgCourse, IFormFile CourseDemo)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "no-photo.jpg";
            //TOdoCheck image
            if (imgCourse != null && imgCourse.IsImage())
            {

                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }
                #region ImgThumb
                ImageConvertor imgResizer = new ImageConvertor();
                string thumbpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);

                imgResizer.Image_resize(imagepath, thumbpath, 150);
                #endregion

            }

            if (CourseDemo != null)
            {
                course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(CourseDemo.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    CourseDemo.CopyTo(stream);
                }

            }

            _context.Courses.Add(course);
            _context.SaveChanges();

            return course.CourseId;
        }

        public int AddEpisode(CourseEpisode episode, IFormFile fileEpisode)
        {
            episode.EpisodeFileName = fileEpisode.FileName;


            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/coursefile", episode.EpisodeFileName);
            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                fileEpisode.CopyTo(stream);
            }


            _context.CourseEpisodes.Add(episode);
            _context.SaveChanges();
            return episode.EpisodeId;
        }

        public bool CheckExistFile(string fileName)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/coursefile", fileName);
            return File.Exists(path);
        }

        public List<CourseGroup> GetAllGroup()
        {
            return _context.courseGroups.ToList();
        }

        public List<ShowCourseForAdminViewModel> GetCorsesForAdmin()
        {
            return _context.Courses.Select(x => new ShowCourseForAdminViewModel()
            {
                CourseId = x.CourseId,
                EpisodeCount = x.CourseEpisodes.Count,
                ImageName = x.CourseImageName,
                Title = x.CourseTitle,
            }).ToList();

        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _context.courseGroups.Where(g => g.ParentId == null).Select(x => new SelectListItem
            {
                Text = x.GroupTitle,
                Value = x.GroupId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetLevels()
        {
            return _context.CourseLevels.Select(l => new SelectListItem()
            {
                Text = l.LevelTitle,
                Value = l.LevelId.ToString()
            }).ToList();
        }

        public List<CourseEpisode> GetListEpisodeCourse(int courseId)
        {
            return _context.CourseEpisodes.Where(c => c.CourseId == courseId).ToList();
        }

        public List<SelectListItem> GetStatues()
        {
            return _context.CourseStatuses.Select(s => new SelectListItem()
            {
                Text = s.StatusTitle,
                Value = s.StatusId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetSubGroupForManageCourse(int groupId)
        {
            return _context.courseGroups.Where(g => g.ParentId == groupId).Select(x => new SelectListItem
            {
                Text = x.GroupTitle,
                Value = x.GroupId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRoles.Where(x => x.RoleId == 2).Include(x => x.User)
                .Select(x => new SelectListItem()
                {
                    Value = x.UR_Id.ToString(),
                    Text = x.User.UserName
                }).ToList();
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile demo)
        {
            course.CreateDate = DateTime.Now;
            //TOdoCheck image
            if (imgCourse != null && imgCourse.IsImage())
            {
                if (course.CourseImageName == "no-photo.jpg")
                {
                    var deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }
                    var deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }
                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(imgCourse.FileName);
                string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/image", course.CourseImageName);
                using (var stream = new FileStream(imagepath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }
                #region ImgThumb
                ImageConvertor imgResizer = new ImageConvertor();
                string thumbpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);

                imgResizer.Image_resize(imagepath, thumbpath, 150);
                #endregion

            }

            if (demo != null)
            {
                if (course.DemoFileName != null)
                {
                    var deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/demoes", course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }
                }
                course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(demo.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demo.CopyTo(stream);
                }

            }
            _context.Update(course);
            _context.SaveChanges();
        }

        public CourseEpisode GetEpisodeById(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public void EditEpisode(CourseEpisode episode, IFormFile episodeFile)
        {
            if (episodeFile != null)
            {
                string deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", episode.EpisodeFileName);
                File.Delete(deleteFilePath);

                episode.EpisodeFileName = episodeFile.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", episode.EpisodeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }

            _context.CourseEpisodes.Update(episode);
            _context.SaveChanges();

        }
    }
}
