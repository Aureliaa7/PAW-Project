using System;

namespace UniversityApp.Core.DTOs
{
    public class EnrollmentDto
    { 
        public Guid EnrollmentId { get; set; }

        public Guid CourseId { get; set; }

        public Guid StudentId { get; set; }
    }
}
