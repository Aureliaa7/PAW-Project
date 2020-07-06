﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Models
{
    public partial class Students
    {
        public Students()
        {
            Enrollments = new HashSet<Enrollments>();
        }

        [Key]
        [Display(Name = "Student ID")]
        public int StudentId { get; set; }
        [ForeignKey("Users")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [StringLength(13)]
        public string Cnp { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Display(Name = "Study Year")]
        public int StudyYear { get; set; }
        public string Section { get; set; }
        [Display(Name = "Group Name")]
        public string GroupName { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<Enrollments> Enrollments { get; set; }
    }
}
