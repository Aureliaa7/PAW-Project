using System;
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
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork unitOfWork;

        public TeacherService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddAsync(TeacherRegistrationViewModel teacherModel,Guid userId)
        {
            Teacher teacher = new Teacher
            {
                FirstName = teacherModel.FirstName,
                LastName = teacherModel.LastName,
                Cnp = teacherModel.Cnp,
                PhoneNumber = teacherModel.PhoneNumber,
                Email = teacherModel.Email,
                Degree = teacherModel.Degree
            };
            await unitOfWork.TeachersRepository.CreateAsync(teacher);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool teacherExists = await unitOfWork.TeachersRepository.ExistsAsync(t => t.Id == id);
            if (!teacherExists)
            {
                throw new EntityNotFoundException($"The teacher with the id {id} was not found!");
            }
            await unitOfWork.TeachersRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IQueryable<Teacher>> GetAsync(Expression<Func<Teacher, bool>> filter = null)
        {
            return await unitOfWork.TeachersRepository.GetAsync(filter);
        }

        public async Task<Teacher> GetFirstOrDefaultAsync(Expression<Func<Teacher, bool>> filter)
        {
            return (await unitOfWork.TeachersRepository.GetAsync(filter)).FirstOrDefault();
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            bool teacherExists = await unitOfWork.TeachersRepository.ExistsAsync(t => t.Id == teacher.Id);
            if (!teacherExists)
            {
                throw new EntityNotFoundException($"The teached with the id {teacher.Id} was not found!");
            }
            await unitOfWork.TeachersRepository.UpdateAsync(teacher);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
