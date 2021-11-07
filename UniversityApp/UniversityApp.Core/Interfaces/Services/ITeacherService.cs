using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ITeacherService
    {
        Task AddAsync(TeacherRegistrationViewModel teacherModel, Guid userId);

        Task<IQueryable<Teacher>> GetAsync(Expression<Func<Teacher, bool>> filter = null);

        Task UpdateAsync(Teacher teacher);

        Task DeleteAsync(Guid id);

        Task<Teacher> GetFirstOrDefaultAsync(Expression<Func<Teacher, bool>> filter);
    }
}
