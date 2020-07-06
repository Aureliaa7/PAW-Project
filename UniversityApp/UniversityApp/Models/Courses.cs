using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Models
{
    public partial class Courses
    {
        public Courses()
        {
            Enrollments = new HashSet<Enrollments>();
            TeachedCourses = new HashSet<TeachedCourses>();
        }

        [Key]
        [Display(Name = "Course ID")]
        public int CourseId { get; set; }
        [StringLength(50)]
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        [Display(Name = "Credits")]
        [Range(0, 5)]
        public int NoCredits { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }

        public virtual ICollection<Enrollments> Enrollments { get; set; }
        public virtual ICollection<TeachedCourses> TeachedCourses { get; set; }
    }
}
