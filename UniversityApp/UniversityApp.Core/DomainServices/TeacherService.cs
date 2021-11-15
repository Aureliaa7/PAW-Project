using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Exceptions;
using UniversityApp.Core.Interfaces.Repositories;
using UniversityApp.Core.Interfaces.Services;

namespace UniversityApp.Core.DomainServices
{
    public class TeacherService : UserService, ITeacherService
    {
        public TeacherService(IUnitOfWork unitOfWork, UserManager<User> userManager) 
            : base(unitOfWork, userManager) { }

        public async Task AddAsync(Teacher teacherModel, string password)
        {
            var errorMessages = await SaveUserAsync(teacherModel, password, Constants.TeacherRole);
            if (errorMessages.Any())
            {
                throw new FailedUserRegistrationException(string.Join("\n", errorMessages));
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await CheckIfTeacherExistsAsync(id);
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
            await CheckIfTeacherExistsAsync(teacher.Id);
            var existingTeacher = await GetFirstOrDefaultAsync(x => x.Id == teacher.Id);
            SetNewPropertyValues(existingTeacher, teacher);
            existingTeacher.Degree = teacher.Degree;
            await unitOfWork.TeachersRepository.UpdateAsync(existingTeacher);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task CheckIfTeacherExistsAsync(Guid id)
        {
            bool teacherExists = await unitOfWork.TeachersRepository.ExistsAsync(t => t.Id == id);
            if (!teacherExists)
            {
                throw new EntityNotFoundException($"The teached with the id {id} was not found!");
            }
        }
    }
}
