using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Models
{
    public partial class Teachers
    {
        public Teachers()
        {
            TeachedCourses = new HashSet<TeachedCourses>();
        }

        [Key]
        [Display(Name = "Teacher ID")]
        public int TeacherId { get; set; }
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

        public virtual Users User { get; set; }
        public virtual ICollection<TeachedCourses> TeachedCourses { get; set; }
    }
}
