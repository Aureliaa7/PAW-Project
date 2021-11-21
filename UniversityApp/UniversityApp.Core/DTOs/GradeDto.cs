using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityApp.Core.DTOs
{
    public class GradeDto
    {
        public Guid GradeId { get; set; }

        [Required]
        public Guid EnrollmentId { get; set; }

        [Required]
        public int? Value { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        public EnrollmentDto Enrollment { get; set; }
    }
}
