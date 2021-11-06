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

        //TODO get rid of this one
        public async Task DeleteAsync(DeleteStudentViewModel model)
        {
            var studentToBeDeleted = (await unitOfWork.StudentsRepository.FindAsync(s => (s.StudyYear == model.StudyYear)
                && (String.Equals(s.Section, model.SectionName)) && (String.Equals(s.Cnp, model.Cnp)))).FirstOrDefault();
            if (studentToBeDeleted != null)
            {
                await unitOfWork.StudentsRepository.DeleteAsync(studentToBeDeleted);
                await unitOfWork.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            bool studentExists = await unitOfWork.StudentsRepository.ExistsAsync(s => s.Id == id);
            if (!studentExists)
            {
                throw new EntityNotFoundException($"The student with the id {id} was not found!");
            }

            await unitOfWork.StudentsRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Student>> GetAsync(Expression<Func<Student, bool>> filter = null)
        {
            return (await unitOfWork.StudentsRepository.FindAsync(filter)).ToList();
        }

        public async Task UpdateAsync(Student student)
        {
            bool studentExists = await unitOfWork.StudentsRepository.ExistsAsync(s => s.Id == student.Id);
            if (!studentExists)
            {
                throw new EntityNotFoundException($"The student with the id {student.Id} was not found!");
            }

            await unitOfWork.StudentsRepository.DeleteAsync(student.Id);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
