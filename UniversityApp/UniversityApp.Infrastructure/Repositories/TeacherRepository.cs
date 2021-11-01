using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces;


namespace UniversityApp.Repositories
{
    //TODO delete this as well
    public class TeacherRepository : RepositoryBase<Teachers>, ITeacherRepository
    {
        public TeacherRepository(UniversityAppContext context) : base(context) { }
    }
}
