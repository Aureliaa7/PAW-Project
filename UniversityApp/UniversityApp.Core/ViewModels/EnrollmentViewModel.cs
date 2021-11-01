using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.ViewModels
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
