using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Repositories
{
    public class EnrollmentRepository : RepositoryBase<Enrollments>, IEnrollmentRepository
    {
        public EnrollmentRepository(UniversityAppContext context) : base(context) { }

        public async Task<IEnumerable<Enrollments>> GetEnrollments(Guid studentId)
        {
            var enrollments = (await FindAsync(enrollment => enrollment.StudentId == studentId)).Select(enrollment => enrollment).ToList();
            return enrollments;
        }
    }
}
