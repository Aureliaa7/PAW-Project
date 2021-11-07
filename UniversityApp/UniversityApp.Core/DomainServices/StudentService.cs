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
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddAsync(StudentRegistrationViewModel studentModel, Guid userId)
        {
            Student student = new Student
            {
                FirstName = studentModel.FirstName,
                LastName = studentModel.LastName,
                Cnp = studentModel.Cnp,
                PhoneNumber = studentModel.PhoneNumber,
                Email = studentModel.Email,
                StudyYear = studentModel.StudyYear,
                Section = studentModel.Section,
                GroupName = studentModel.GroupName
            };
            await unitOfWork.StudentsRepository.CreateAsync(student);
            await unitOfWork.SaveChangesAsync();
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

        public async Task<Student> GetFirstOrDefaultAsync(Expression<Func<Student, bool>> filter)
        {
            return (await unitOfWork.StudentsRepository.GetAsync(filter)).FirstOrDefault();
        }

        public async Task UpdateAsync(Student student)
        {
            await CheckIfStudentExistsAsync(student.Id);
            await unitOfWork.StudentsRepository.DeleteAsync(student.Id);
            await unitOfWork.SaveChangesAsync();
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
