using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.ViewModels
{
    public class CreateEnrollmentViewModel
    {
        [Required]
        [Display(Name = "Course")]
        public Guid CourseId { get; set; }

        [Required]
        [Display(Name = "Teacher")]
        public Guid TeacherId { get; set; }

        [Required]
        [Display(Name = "Student")]
        public string StudentCnp { get; set; }
    }
}
