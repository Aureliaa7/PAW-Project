using System;

namespace UniversityApp.Core.DomainEntities
{
    public class Grade
    {
        public Guid GradeId { get; set; }

        public Guid EnrollmentId { get; set; }

        public int? Value { get; set; }

        public DateTime? Date { get; set; }

        public virtual Enrollment Enrollment { get; set; }
    }
}
