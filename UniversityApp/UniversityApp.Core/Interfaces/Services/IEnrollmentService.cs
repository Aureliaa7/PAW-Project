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
        Task<IEnumerable<Enrollment>> GetEnrollmentsForStudentAsync(Guid studentId);

        Task<IEnumerable<EnrollmentViewModel>> GetAllEnrollmentsAsync();

        Task DeleteEnrollmentAsync(DeleteEnrollmentViewModel model);

        Task CreateEnrollmentAsync(EnrollmentViewModel model);

        Task<Enrollment> GetFirstOrDefaultAsync(Expression<Func<Enrollment, bool>> filter);

        Task<IEnumerable<Enrollment>> GetAsync(Expression<Func<Enrollment, bool>> filter = null);
    }
}
