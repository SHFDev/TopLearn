using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseGroup
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "لطفا {} را وارد کنید ")]
        [Display(Name = "عنوان گروه ")]
        [MaxLength(200, ErrorMessage = "{0}نمیتواند بیشتر از {1} کاراکتر باشد")]
        public string GroupTitle { get; set; }
       
        
        [Display(Name = "حذف شده ؟ ")]
        public bool IsDelete { get; set; }
       
        
        [Display(Name = "گروه اصلی ")]
        public int? ParentId { get; set; }


        [ForeignKey("ParentId")]
        public List<CourseGroup> courseGroups { get; set; }
    }
}
