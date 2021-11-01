using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ICourseService
    {
        // get all the students taking a course
        Task<IEnumerable<Students>> GetEnrolledStudents(Guid courseId);

        Task<Courses> AddAsync(Courses course);

        Task DeleteAsync(Guid id);

        Task<Courses> UpdateAsync(Courses course);

        Task<IEnumerable<Courses>> GetAsync(Expression<Func<Courses, bool>> filter = null);

        Task<Courses> GetFirstOrDefaultAsync(Expression<Func<Courses, bool>> filter);
    }
}
