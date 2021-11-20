using System;
using System.Collections.Generic;

namespace UniversityApp.Core.DomainEntities
{
    public class Course
    {
        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
            TeachedCourses = new HashSet<TeachedCourse>();
        }

        public Guid Id { get; set; }

        public string CourseTitle { get; set; }

        public int NoCredits { get; set; }

        public int Year { get; set; }

        public int Semester { get; set; }

        
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public virtual ICollection<TeachedCourse> TeachedCourses { get; set; }
    }
}
