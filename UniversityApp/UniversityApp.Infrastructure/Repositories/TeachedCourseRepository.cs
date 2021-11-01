using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Repositories
{
    //TODO delete this as well
    public class TeachedCourseRepository : RepositoryBase<TeachedCourses>, ITeachedCourseRepository
    {
        public TeachedCourseRepository(UniversityAppContext context):base(context) { }
    }
}
