using System;
using System.Collections.Generic;

namespace UniversityApp.Core.DomainEntities
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            Grades = new HashSet<Grade>();
        }

        public Guid EnrollmentId { get; set; }

        public Guid CourseId { get; set; }

        public Guid StudentId { get; set; }


        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}
