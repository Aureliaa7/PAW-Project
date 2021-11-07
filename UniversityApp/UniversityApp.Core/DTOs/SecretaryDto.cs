using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.DTOs
{
    public class SecretaryDto
    {
        public Guid Id { get; set; }

        public byte[] Image { get; set; }

        public string Cnp { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
