using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;
using UniversityApp.Models;
using UniversityApp.Repositories;
using UniversityApp.ViewModels;

namespace UniversityApp.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        public EnrollmentService(UniversityAppContext context)
        {
            EnrollmentRepository = new EnrollmentRepository(context);
            CourseRepository = new CourseRepository(context);
            StudentRepository = new StudentRepository(context);
        }

        public IEnrollmentRepository EnrollmentRepository { get; }
        public ICourseRepository CourseRepository { get; }
        public IStudentRepository StudentRepository { get; }

        public IEnumerable<Enrollments> GetEnrollmentsForStudent(int studentId)
        {
            return EnrollmentRepository.GetEnrollments(studentId);
        }

        public void DeleteEnrollment(DeleteEnrollmentViewModel model)
        {
            var course = CourseRepository.FindByCondition(c => String.Equals(c.CourseTitle, model.CourseTitle)).FirstOrDefault();
            var enrolledStudent = StudentRepository.FindByCondition(s => String.Equals(s.Cnp, model.StudentCNP)).FirstOrDefault();

            if (course != null && enrolledStudent != null)
            {
                var enrollment = EnrollmentRepository.FindByCondition(e => (e.CourseId == course.CourseId) && (e.StudentId == enrolledStudent.StudentId)).FirstOrDefault();
                if (enrollment != null)
                {
                    EnrollmentRepository.Delete(enrollment);
                }
            }
        }

        public IEnumerable<EnrollmentViewModel> GetAllEnrollments()
        {
            var enrollments = EnrollmentRepository.FindAll().ToList();
            var enrollmentModels = new List<EnrollmentViewModel>();
            if (enrollments != null)
            {
                foreach (var enrollment in enrollments)
                {
                    Courses course = CourseRepository.FindByCondition(c => c.CourseId == enrollment.CourseId).FirstOrDefault();
                    Students student = StudentRepository.FindByCondition(s => String.Equals(s.StudentId, enrollment.StudentId)).FirstOrDefault();
                    if (course != null && student != null)
                    {
                        enrollmentModels.Add(new EnrollmentViewModel
                        {
                            CourseTitle = course.CourseTitle,
                            StudentCnp = student.Cnp,
                            FirstName = student.FirstName,
                            LastName = student.LastName
                        });
                    }
                }
            }
            return enrollmentModels;
        }

        public void CreateEnrollment(EnrollmentViewModel model)
        {
            Courses course = CourseRepository.FindByCondition(c => String.Equals(c.CourseTitle, model.CourseTitle)).FirstOrDefault();
            Students student = StudentRepository.FindByCondition(s => String.Equals(s.Cnp, model.StudentCnp)).FirstOrDefault();
            if (course != null && student != null)
            {
                var foundEnrollment = EnrollmentRepository.FindByCondition(e => (e.CourseId == course.CourseId) && (String.Equals(e.Student.Cnp, student.Cnp))).FirstOrDefault();
                if(foundEnrollment == null)
                {
                    Enrollments enrollment = new Enrollments
                    {
                        CourseId = course.CourseId,
                        StudentId = student.StudentId
                    };
                    EnrollmentRepository.Create(enrollment);
                }
            }
        }
    }
}
