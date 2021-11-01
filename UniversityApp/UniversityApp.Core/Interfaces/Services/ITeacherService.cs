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
        Task AddAsync(TeacherRegistrationViewModel teacherModel, string userId);

        Task<IQueryable<Teachers>> GetAsync(Expression<Func<Teachers, bool>> filter = null);

        Task UpdateAsync(Teachers teacher);

        Task DeleteAsync(Guid id);
    }
}
