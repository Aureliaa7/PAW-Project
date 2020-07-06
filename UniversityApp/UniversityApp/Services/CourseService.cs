using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.Repositories;

namespace UniversityApp.Services
{
    public class CourseService : ICourseService
    {
        public CourseService(UniversityAppContext context)
        {
            EnrollmentRepository = new EnrollmentRepository(context);
            CourseRepository = new CourseRepository(context);
        }

        public IEnrollmentRepository EnrollmentRepository { get; }

        public ICourseRepository CourseRepository { get; }

        // returns all the students taking a certain course
        public IEnumerable<Students> GetEnrolledStudents(int courseId)
        {
            var enrollments = EnrollmentRepository.FindByCondition(enrollment => enrollment.CourseId == courseId);
            var enrolledStudents = enrollments.Select(enrollment => enrollment.Student).ToList();
            return enrolledStudents;
        }
    }
}
