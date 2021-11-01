using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.DomainEntities
{
    public partial class Enrollments
    {
        public Enrollments()
        {
            Grades = new HashSet<Grades>();
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

        public virtual Courses Course { get; set; }
        public virtual Students Student { get; set; }
        public virtual ICollection<Grades> Grades { get; set; }
    }
}
