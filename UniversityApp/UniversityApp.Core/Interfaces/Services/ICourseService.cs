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
        Task<IEnumerable<Student>> GetEnrolledStudents(Guid courseId);

        Task<Course> AddAsync(Course course);

        Task DeleteAsync(Guid id);

        Task<Course> UpdateAsync(Course course);

        Task<IEnumerable<Course>> GetAsync(Expression<Func<Course, bool>> filter = null);

        Task<Course> GetFirstOrDefaultAsync(Expression<Func<Course, bool>> filter);
    }
}
