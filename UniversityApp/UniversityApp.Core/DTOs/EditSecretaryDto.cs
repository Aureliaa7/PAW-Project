using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.DTOs
{
    public class EditSecretaryDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Cnp { get; set; }

        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
