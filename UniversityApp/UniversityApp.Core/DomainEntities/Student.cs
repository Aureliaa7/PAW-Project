using System.Collections.Generic;

namespace UniversityApp.Core.DomainEntities
{
    public class Student : User
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        public int StudyYear { get; set; }

        public string Section { get; set; }

        public string GroupName { get; set; }


        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
