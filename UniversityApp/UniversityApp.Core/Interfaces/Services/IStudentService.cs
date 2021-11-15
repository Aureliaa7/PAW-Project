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
        Task AddAsync(Student studentModel, string password);

        Task DeleteAsync(DeleteStudentViewModel model);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<Student>> GetAsync(Expression<Func<Student, bool>> filter = null);

        Task<Student> UpdateAsync(Student student);

        Task<Student> GetFirstOrDefaultAsync(Expression<Func<Student, bool>> filter);

        Task<Student> GetEnrolledStudentAsync(string courseTitle, string cnp);
    }
}
