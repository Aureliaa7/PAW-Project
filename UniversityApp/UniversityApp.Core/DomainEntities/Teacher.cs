using System.Collections.Generic;

namespace UniversityApp.Core.DomainEntities
{
    public class Teacher : User
    {
        public Teacher()
        {
            TeachedCourses = new HashSet<TeachedCourse>();
        }

        public string Degree { get; set; }

        public virtual ICollection<TeachedCourse> TeachedCourses { get; set; }
    }
}
