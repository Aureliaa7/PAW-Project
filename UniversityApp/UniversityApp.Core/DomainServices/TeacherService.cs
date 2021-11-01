using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.Interfaces.Services;
using UniversityApp.Core.ViewModels;
using UniversityApp.Interfaces;

namespace UniversityApp.Core.DomainServices
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            this.teacherRepository = teacherRepository;
        }

        public async Task AddAsync(TeacherRegistrationViewModel teacherModel, string userId)
        {
            Teachers teacher = new Teachers
            {
                UserId = userId,
                FirstName = teacherModel.FirstName,
                LastName = teacherModel.LastName,
                Cnp = teacherModel.Cnp,
                PhoneNumber = teacherModel.PhoneNumber,
                Email = teacherModel.Email,
                Degree = teacherModel.Degree
            };
            await teacherRepository.CreateAsync(teacher);
            await teacherRepository.SaveAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            bool teacherExists = await teacherRepository.ExistsAsync(t => t.TeacherId == id);
            if (!teacherExists)
            {
                //TODO throw an exception
            }
            await teacherRepository.DeleteAsync(id);
            await teacherRepository.SaveAsync();
        }

        public async Task<IQueryable<Teachers>> GetAsync(Expression<Func<Teachers, bool>> filter = null)
        {
            return await teacherRepository.FindAsync(filter);
        }

        public async Task UpdateAsync(Teachers teacher)
        {
            bool teacherExists = await teacherRepository.ExistsAsync(t => t.TeacherId == teacher.TeacherId);
            if (!teacherExists)
            {
                //TODO throw an exception
            }
            await teacherRepository.UpdateAsync(teacher);
            await teacherRepository.SaveAsync();
        }
    }
}
