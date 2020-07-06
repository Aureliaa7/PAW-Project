using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniversityApp.ViewModels
{
    public class EnrollmentViewModel
    {
        [Required]
        [Display(Name = "Course title")]
        public string CourseTitle { get; set; }
        [Required]
        [Display(Name = "Student CNP")]
        public string StudentCnp { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
