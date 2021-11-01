using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<Enrollments>> GetEnrollmentsForStudentAsync(Guid studentId);

        Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsAsync();

        Task DeleteEnrollmentAsync(DeleteEnrollmentViewModel model);

        Task CreateEnrollmentAsync(EnrollmentViewModel model);

        Task<Enrollments> GetFirstOrDefaultAsync(Expression<Func<Enrollments, bool>> filter);

        Task<IEnumerable<Enrollments>> GetAsync(Expression<Func<Enrollments, bool>> filter = null);
    }
}
