using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.DomainEntities
{
    public partial class TeachedCourses
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("Teachers")]
        [Display(Name = "Teacher ID")]
        public Guid TeacherId { get; set; }
        [ForeignKey("Courses")]
        [Display(Name = "Course ID")]
        public Guid CourseId { get; set; }

        public virtual Courses Course { get; set; }
        public virtual Teachers Teacher { get; set; }
    }
}
