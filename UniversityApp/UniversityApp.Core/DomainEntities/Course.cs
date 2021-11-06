using System;
using System.Collections.Generic;

namespace UniversityApp.Core.DomainEntities
{
    public class Course
    {
        private int noCredits;
        private int studyYear;
        private int semester;

        public Course()
        {
            Enrollments = new HashSet<Enrollment>();
            TeachedCourses = new HashSet<TeachedCourse>();
        }

        public Guid Id { get; set; }

        public string CourseTitle { get; set; }

        public int NoCredits {
            get
            {
                return noCredits;
            }
            set
            {
                if (value < Constants.MinNumberOfCredits || value > Constants.MaxNumberOfCredits)
                {
                    throw new ArgumentOutOfRangeException("The number of credits should be between" +
                        $" {Constants.MinNumberOfCredits} and {Constants.MaxNumberOfCredits}");
                }

                noCredits = value;
            }
        }

        public int Year {
            get
            {
                return studyYear;
            }
            set
            {
                if (value < Constants.MinStudyYear || value > Constants.MaxStudyYear)
                {
                    throw new ArgumentOutOfRangeException("The study year should be between" +
                        $" {Constants.MinStudyYear} and {Constants.MaxStudyYear}");
                }

                studyYear = value;
            }
        }

        public int Semester {
            get
            {
                return semester;
            }
            set
            {
                if (value < 1 || value > 2)
                {
                    throw new ArgumentOutOfRangeException("The semester should be 1 or 2");
                }

                semester = value;
            }
        }

        
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        public virtual ICollection<TeachedCourse> TeachedCourses { get; set; }
    }
}
