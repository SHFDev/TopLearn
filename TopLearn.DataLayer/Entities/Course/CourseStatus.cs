using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseStatus
    {
        [Key]
        public int StatusId { get; set; }
        
        
        [MaxLength(150, ErrorMessage = "{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {} را وارد کنید ")]
        public string StatusTitle { get; set; }
        public List<Course> Courses { get; set; }

    }
}
