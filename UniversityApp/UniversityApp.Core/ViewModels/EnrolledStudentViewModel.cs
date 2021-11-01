using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.ViewModels
{
    public class EnrolledStudentViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string CNP { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Section { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
        [Display(Name = "Enrollment ID")]
        public Guid EnrollmentID { get; set; }
    }
}
