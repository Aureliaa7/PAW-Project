using System;

namespace UniversityApp.Core.DTOs
{
    public class GradeDto
    {
        public Guid GradeId { get; set; }

        public Guid EnrollmentId { get; set; }

        public int? Value { get; set; }

        public DateTime? Date { get; set; }

        public EnrollmentDto Enrollment { get; set; }
    }
}
