﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.ViewModels
{
    public class StudentRegistrationViewModel
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(13)]
        public string Cnp { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Display(Name = "Study year")]
        public int StudyYear { get; set; }
        
        [Required]
        public string Section { get; set; }

        [Required]
        [Display(Name = "Group name")]
        public string GroupName { get; set; }
        
        [Required]
        [MinLength(7)]
        [DataType(DataType.Password)]
        [Display(Name = "Access Code")]
        public string Password { get; set; }
        
        public IFormFile Image { get; set; }
    }
}
