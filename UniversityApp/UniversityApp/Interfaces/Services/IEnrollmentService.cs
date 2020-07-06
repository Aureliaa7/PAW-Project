using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.ViewModels;

namespace UniversityApp.Interfaces
{
    public interface IEnrollmentService
    {
        IEnrollmentRepository EnrollmentRepository { get; }
        ICourseRepository CourseRepository { get; }
        IStudentRepository StudentRepository { get; }

        IEnumerable<Enrollments> GetEnrollmentsForStudent(int studentId);
        IEnumerable<EnrollmentViewModel> GetAllEnrollments();
        void DeleteEnrollment(DeleteEnrollmentViewModel model);
        void CreateEnrollment(EnrollmentViewModel model);
    }
}
