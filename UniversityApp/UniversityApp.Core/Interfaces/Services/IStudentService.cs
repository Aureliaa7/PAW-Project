using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IStudentService
    {
        Task AddAsync(StudentRegistrationViewModel studentModel, string userId);

        Task DeleteAsync(DeleteStudentViewModel model);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<Students>> GetAsync(Expression<Func<Students, bool>> filter = null);

        Task UpdateAsync(Students student);
    }
}
