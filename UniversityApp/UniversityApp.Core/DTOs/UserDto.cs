using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "First name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Cnp { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
