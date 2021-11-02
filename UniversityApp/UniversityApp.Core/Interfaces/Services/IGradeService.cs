using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IGradeService
    {
        Task<IEnumerable<StudentGrade>> GetGradesForStudentAsync(Guid studentId);

        Task<Grade> GetFirstOrDefaultAsync(Expression<Func<Grade, bool>> filter);

        Task<IEnumerable<Grade>> GetAllAsync();

        Task AddAsync(Grade grade);

        Task UpdateAsync(Grade grade);

        Task DeleteAsync(Guid id);
    }
}
