using Microsoft.AspNetCore.Identity;
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
    public class StudentService : UserService, IStudentService
    {
        public StudentService(IUnitOfWork unitOfWork, UserManager<User> userManager) : base(unitOfWork, userManager) { }

        public async Task AddAsync(Student studentModel, string password)
        {
            var errorMessages = await SaveUserAsync(studentModel, password, Constants.StudentRole);
            if (errorMessages.Any())
            {
                throw new FailedUserRegistrationException(string.Join("\n", errorMessages));
            }
        }

        public async Task DeleteAsync(DeleteStudentViewModel model)
        {
            var studentToBeDeleted = (await unitOfWork.StudentsRepository.GetAsync(s => (s.StudyYear == model.StudyYear)
                && (String.Equals(s.Section, model.SectionName)) && (String.Equals(s.Cnp, model.Cnp)))).FirstOrDefault();
            if (studentToBeDeleted == null)
            {
                throw new EntityNotFoundException($"The student was not found!");
            }

            await unitOfWork.StudentsRepository.DeleteAsync(studentToBeDeleted.Id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await CheckIfStudentExistsAsync(id);
            await unitOfWork.StudentsRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAsync(Expression<Func<Student, bool>> filter = null)
        {
            return (await unitOfWork.StudentsRepository.GetAsync(filter)).ToList();
        }

        public async Task<Student> GetEnrolledStudentAsync(string courseTitle, string cnp)
        {
            var course = (await unitOfWork.CoursesRepository.GetAsync(c => c.CourseTitle == courseTitle)).FirstOrDefault();
            var student = (await unitOfWork.StudentsRepository.GetAsync(s => s.Cnp == cnp)).FirstOrDefault();

            if (course != null && student != null)
            {
                var enrollment = (await unitOfWork.EnrollmentsRepository.GetAsync(
                    e => e.CourseId == course.Id && e.StudentId == student.Id)).FirstOrDefault();
                if (enrollment != null)
                {
                    return student;
                }
            }
            return null;
        }

        public async Task<Student> GetFirstOrDefaultAsync(Expression<Func<Student, bool>> filter)
        {
            return (await unitOfWork.StudentsRepository.GetAsync(filter)).FirstOrDefault();
        }

        public async Task<Student> UpdateAsync(Student student)
        {
            await CheckIfStudentExistsAsync(student.Id);
            var existingStudent = await GetFirstOrDefaultAsync(x => x.Id == student.Id);
            SetNewPropertyValues(existingStudent, student);
            existingStudent.Section = student.Section;
            existingStudent.GroupName = student.GroupName;
            existingStudent.StudyYear = student.StudyYear;
            var updatedStudent = await unitOfWork.StudentsRepository.UpdateAsync(existingStudent);
            await unitOfWork.SaveChangesAsync();

            return updatedStudent;
        }

        private async Task CheckIfStudentExistsAsync(Guid id)
        {
            bool studentExists = await unitOfWork.StudentsRepository.ExistsAsync(s => s.Id == id);
            if (!studentExists)
            {
                throw new EntityNotFoundException($"The student with the id {id} was not found!");
            }
        }
    }
}
