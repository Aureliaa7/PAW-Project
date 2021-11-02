using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Core.DomainEntities
{
    public partial class Grade
    {
        [Key]
        [Display(Name = "Grade ID")]
        public Guid GradeId { get; set; }
        [ForeignKey("Enrollments")]
        [Display(Name = "Enrollment ID")]
        public Guid EnrollmentId { get; set; }
        public int? Value { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public virtual Enrollment Enrollment { get; set; }
    }
}
