using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Interfaces;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Core.DomainServices
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository enrollmentRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IStudentRepository studentRepository;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            ICourseRepository courseRepository,
            IStudentRepository studentRepository
            )
        {
            this.enrollmentRepository = enrollmentRepository;
            this.courseRepository = courseRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Enrollments>> GetEnrollmentsForStudentAsync(Guid studentId)
        {
            return (await enrollmentRepository.FindAsync(e => e.StudentId == studentId)).ToList();
        }

        public async Task DeleteEnrollmentAsync(DeleteEnrollmentViewModel model)
        {
            var course = (await courseRepository.FindAsync(c => String.Equals(c.CourseTitle, model.CourseTitle))).FirstOrDefault();
            var enrolledStudent = (await studentRepository.FindAsync(s => String.Equals(s.Cnp, model.StudentCNP))).FirstOrDefault();

            if (course != null && enrolledStudent != null)
            {
                var enrollment = (await enrollmentRepository
                    .FindAsync(e => (e.CourseId == course.CourseId) && 
                        (e.StudentId == enrolledStudent.StudentId)))
                    .FirstOrDefault();
                if (enrollment != null)
                {
                    await enrollmentRepository.DeleteAsync(enrollment.EnrollmentId);
                    await enrollmentRepository.SaveAsync();
                }
            }
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsAsync()
        {
            var enrollments = (await enrollmentRepository.FindAsync()).ToList();
            var enrollmentModels = new List<EnrollmentViewModel>();
            if (enrollments != null)
            {
                foreach (var enrollment in enrollments)
                {
                    Courses course = (await courseRepository.FindAsync(c => c.CourseId == enrollment.CourseId)).FirstOrDefault();
                    Students student = (await studentRepository.FindAsync(s => String.Equals(s.StudentId, enrollment.StudentId))).FirstOrDefault();
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

        public async Task CreateEnrollmentAsync(EnrollmentViewModel model)
        {
            Courses course = (await courseRepository.FindAsync(c => String.Equals(c.CourseTitle, model.CourseTitle))).FirstOrDefault();
            Students student = (await studentRepository.FindAsync(s => String.Equals(s.Cnp, model.StudentCnp))).FirstOrDefault();
            if (course != null && student != null)
            {
                var foundEnrollment = (await enrollmentRepository.FindAsync(e => (e.CourseId == course.CourseId) 
                && (String.Equals(e.Student.Cnp, student.Cnp)))).FirstOrDefault();
                if(foundEnrollment == null)
                {
                    Enrollments enrollment = new Enrollments
                    {
                        CourseId = course.CourseId,
                        StudentId = student.StudentId
                    };
                    await enrollmentRepository.CreateAsync(enrollment);
                    await enrollmentRepository.SaveAsync();
                }
            }
        }

        public async Task<Enrollments> GetFirstOrDefaultAsync(Expression<Func<Enrollments, bool>> filter)
        {
            return (await enrollmentRepository.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<Enrollments>> GetAsync(Expression<Func<Enrollments, bool>> filter = null)
        {
            return (await enrollmentRepository.FindAsync(filter)).ToList();
        }
    }
}
