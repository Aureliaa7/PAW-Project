using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.DomainEntities
{
    // why are these classes partial?? I don't remember why I made them partial... it doesn't make sense now...

    public partial class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
            TeachedCourses = new HashSet<TeachedCourse>();
        }

        [Key]
        [Display(Name = "Course ID")]
        public Guid CourseId { get; set; }
        [StringLength(50)]
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }
        [Display(Name = "Credits")]
        [Range(0, 5)]
        public int NoCredits { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<TeachedCourse> TeachedCourses { get; set; }
    }
}
