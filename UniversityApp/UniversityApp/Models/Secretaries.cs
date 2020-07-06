using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Models
{
    public partial class Secretaries
    {
        [Key]
        [Display(Name = "ID")]
        public int SecretaryId { get; set; }
        [StringLength(13)]
        public string Cnp { get; set; }
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [ForeignKey("Users")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public virtual Users User { get; set; }
    }
}
