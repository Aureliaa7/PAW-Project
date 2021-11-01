using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Repositories
{
    public class CourseRepository : RepositoryBase<Courses>, ICourseRepository
    {
        public CourseRepository(UniversityAppContext context) : base(context) { }
    }
}
