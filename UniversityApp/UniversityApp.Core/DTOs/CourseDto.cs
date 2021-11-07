using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string CourseTitle { get; set; }

        [Required]
        [Display(Name = "Number of credits")]
        public int NoCredits { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Semester { get; set; }

        //TODO check if these navigation properties are indeed needed
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public virtual ICollection<TeachedCourse> TeachedCourses { get; set; }
    }
}
