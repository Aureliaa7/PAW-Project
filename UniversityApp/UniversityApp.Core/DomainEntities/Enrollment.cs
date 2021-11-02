using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.DomainEntities
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            Grades = new HashSet<Grade>();
        }

        [Key]
        [Display(Name = "Enrollment ID")]
        public Guid EnrollmentId { get; set; }
        [ForeignKey("Courses")]
        [Display(Name = "Course ID")]
        public Guid CourseId { get; set; }
        [ForeignKey("Students")]
        [Display(Name = "Student ID")]
        public Guid StudentId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
