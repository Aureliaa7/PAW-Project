using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
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
            return (await unitOfWork.EnrollmentsRepository.GetAsync(e => e.StudentId == studentId)).ToList();
        }

        public async Task DeleteEnrollmentAsync(DeleteEnrollmentViewModel model)
        {
            var course = (await unitOfWork.CoursesRepository.GetAsync(c => String.Equals(c.CourseTitle, model.CourseTitle))).FirstOrDefault();
            var enrolledStudent = (await unitOfWork.StudentsRepository.GetAsync(s => String.Equals(s.Cnp, model.StudentCNP))).FirstOrDefault();

            if (course != null && enrolledStudent != null)
            {
                var enrollment = (await unitOfWork.EnrollmentsRepository
                    .GetAsync(e => (e.CourseId == course.Id) && 
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
            var enrollments = (await unitOfWork.EnrollmentsRepository.GetAsync()).ToList();
            var enrollmentModels = new List<EnrollmentViewModel>();
            if (enrollments != null)
            {
                foreach (var enrollment in enrollments)
                {
                    Course course = (await unitOfWork.CoursesRepository.GetAsync(c => c.Id == enrollment.CourseId)).FirstOrDefault();
                    Student student = (await unitOfWork.StudentsRepository.GetAsync(s => String.Equals(s.Id, enrollment.StudentId))).FirstOrDefault();
                    if (course != null && student != null)
                    {
                        enrollmentModels.Add(new EnrollmentViewModel
                        {
                            CourseTitle = course.CourseTitle,
                            StudentCnp = student.Cnp,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            EnrollmentId = enrollment.EnrollmentId
                        });
                    }
                }
            }
            return enrollmentModels;
        }

        public async Task CreateEnrollmentAsync(CreateEnrollmentViewModel model)
        {
            await CheckIfEntitiesExists(model);

            Student student = (await unitOfWork.StudentsRepository.GetAsync(s => s.Cnp == model.StudentCnp)).FirstOrDefault();
                var foundEnrollment = (await unitOfWork.EnrollmentsRepository.GetAsync(e => (e.CourseId == model.CourseId) 
                && (String.Equals(e.Student.Cnp, student.Cnp)))).FirstOrDefault();
            if (foundEnrollment == null)
            {
                Enrollment enrollment = new Enrollment
                {
                    CourseId = model.CourseId,
                    StudentId = student.Id,
                    TeacherId = model.TeacherId
                };
                await unitOfWork.EnrollmentsRepository.CreateAsync(enrollment);
                await unitOfWork.SaveChangesAsync();
            }
        }

        private async Task CheckIfEntitiesExists(CreateEnrollmentViewModel model)
        {
            bool studentExists = await unitOfWork.StudentsRepository.ExistsAsync(s => s.Cnp == model.StudentCnp);
            if (!studentExists)
            {
                throw new EntityNotFoundException("The student does not exist!");
            }

            bool courseExists = await unitOfWork.CoursesRepository.ExistsAsync(c => c.Id == model.CourseId);
            if (!courseExists)
            {
                throw new EntityNotFoundException($"The course with the id {model.CourseId} does not exist!");
            }

            bool teacherExists = await unitOfWork.TeachersRepository.ExistsAsync(t => t.Id == model.TeacherId);
            if (!teacherExists)
            {
                throw new EntityNotFoundException($"The teacher with the id {model.TeacherId} does not exist!");
            }     
        }

        public async Task<Enrollment> GetFirstOrDefaultAsync(Expression<Func<Enrollment, bool>> filter)
        {
            return (await unitOfWork.EnrollmentsRepository.GetAsync(filter)).FirstOrDefault();
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync(Expression<Func<Enrollment, bool>> filter = null)
        {
            return (await unitOfWork.EnrollmentsRepository.GetAsync(filter)).ToList();
        }

        public async Task<IEnumerable<EnrolledStudentViewModel>> GetEnrolledStudentsByCourseAndTeacherIdAsync(Guid teacherId, Guid courseId)
        {
            var enrolledStudentsModels = 
                (await unitOfWork.EnrollmentsRepository.GetAsync(e => e.CourseId == courseId && e.TeacherId == teacherId))
                .Select(x => new EnrolledStudentViewModel
                {
                    FirstName = x.Student.FirstName,
                    LastName = x.Student.LastName,
                    CNP = x.Student.Cnp,
                    Email = x.Student.Email,
                    PhoneNumber = x.Student.PhoneNumber,
                    Section = x.Student.Section,
                    GroupName = x.Student.GroupName,
                    EnrollmentID = x.EnrollmentId
                }).ToList();

            return enrolledStudentsModels;
        }
    }
}
