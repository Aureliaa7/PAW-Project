using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces;

namespace UniversityApp.Repositories
{
    public class StudentRepository : RepositoryBase<Students>, IStudentRepository
    {
        public StudentRepository(UniversityAppContext context) : base(context) { }
    }
}
