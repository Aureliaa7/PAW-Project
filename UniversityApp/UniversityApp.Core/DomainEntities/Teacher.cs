using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.DomainEntities
{
    public partial class Teacher
    {
        public Teacher()
        {
            TeachedCourses = new HashSet<TeachedCourse>();
        }

        [Key]
        [Display(Name = "Teacher ID")]
        public Guid TeacherId { get; set; }
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [StringLength(13)]
        public string Cnp { get; set; }
        [ForeignKey("Users")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        public string Degree { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<TeachedCourse> TeachedCourses { get; set; }
    }
}
