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

        Task<Grades> GetFirstOrDefaultAsync(Expression<Func<Grades, bool>> filter);

        Task<IEnumerable<Grades>> GetAllAsync();

        Task AddAsync(Grades grade);

        Task UpdateAsync(Grades grade);

        Task DeleteAsync(Guid id);
    }
}
