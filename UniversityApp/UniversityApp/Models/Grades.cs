using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityApp.Models
{
    public partial class Grades
    {
        [Key]
        [Display(Name = "Grade ID")]
        public int GradeId { get; set; }
        [ForeignKey("Enrollments")]
        [Display(Name = "Enrollment ID")]
        public int EnrollmentId { get; set; }
        public int? Value { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public virtual Enrollments Enrollment { get; set; }
    }
}
