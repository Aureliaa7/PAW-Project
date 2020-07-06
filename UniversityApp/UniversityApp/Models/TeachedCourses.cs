using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Models
{
    public partial class TeachedCourses
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Teachers")]
        [Display(Name = "Teacher ID")]
        public int TeacherId { get; set; }
        [ForeignKey("Courses")]
        [Display(Name = "Course ID")]
        public int CourseId { get; set; }

        public virtual Courses Course { get; set; }
        public virtual Teachers Teacher { get; set; }
    }
}
