using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.ViewModels
{
    public class TeacherRegistrationViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        public string Cnp { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Degree { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Access Code")]
        public string Password { get; set; }
        
        public IFormFile Image { get; set; }
    }
}
