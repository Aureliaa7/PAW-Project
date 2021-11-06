using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.DomainServices
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork unitOfWork;

        public EnrollmentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsForStudentAsync(Guid studentId)
        {
            return (await unitOfWork.EnrollmentsRepository.FindAsync(e => e.StudentId == studentId)).ToList();
        }

        public async Task DeleteEnrollmentAsync(DeleteEnrollmentViewModel model)
        {
            var course = (await unitOfWork.CoursesRepository.FindAsync(c => String.Equals(c.CourseTitle, model.CourseTitle))).FirstOrDefault();
            var enrolledStudent = (await unitOfWork.StudentsRepository.FindAsync(s => String.Equals(s.Cnp, model.StudentCNP))).FirstOrDefault();

            if (course != null && enrolledStudent != null)
            {
                var enrollment = (await unitOfWork.EnrollmentsRepository
                    .FindAsync(e => (e.CourseId == course.Id) && 
                        (e.StudentId == enrolledStudent.Id)))
                    .FirstOrDefault();
                if (enrollment != null)
                {
                    await unitOfWork.EnrollmentsRepository.DeleteAsync(enrollment.EnrollmentId);
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsAsync()
        {
            var enrollments = (await unitOfWork.EnrollmentsRepository.FindAsync()).ToList();
            var enrollmentModels = new List<EnrollmentViewModel>();
            if (enrollments != null)
            {
                foreach (var enrollment in enrollments)
                {
                    Course course = (await unitOfWork.CoursesRepository.FindAsync(c => c.Id == enrollment.CourseId)).FirstOrDefault();
                    Student student = (await unitOfWork.StudentsRepository.FindAsync(s => String.Equals(s.Id, enrollment.StudentId))).FirstOrDefault();
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
            Course course = (await unitOfWork.CoursesRepository.FindAsync(c => String.Equals(c.CourseTitle, model.CourseTitle))).FirstOrDefault();
            Student student = (await unitOfWork.StudentsRepository.FindAsync(s => String.Equals(s.Cnp, model.StudentCnp))).FirstOrDefault();
            if (course != null && student != null)
            {
                var foundEnrollment = (await unitOfWork.EnrollmentsRepository.FindAsync(e => (e.CourseId == course.Id) 
                && (String.Equals(e.Student.Cnp, student.Cnp)))).FirstOrDefault();
                if(foundEnrollment == null)
                {
                    Enrollment enrollment = new Enrollment
                    {
                        CourseId = course.Id,
                        StudentId = student.Id
                    };
                    await unitOfWork.EnrollmentsRepository.CreateAsync(enrollment);
                    await unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<Enrollment> GetFirstOrDefaultAsync(Expression<Func<Enrollment, bool>> filter)
        {
            return (await unitOfWork.EnrollmentsRepository.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<Enrollment>> GetAsync(Expression<Func<Enrollment, bool>> filter = null)
        {
            return (await unitOfWork.EnrollmentsRepository.FindAsync(filter)).ToList();
        }
    }
}
