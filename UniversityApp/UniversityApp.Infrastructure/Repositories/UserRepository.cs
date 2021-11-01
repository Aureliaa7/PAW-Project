using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces.Repositories;

namespace UniversityApp.Repositories
{
    public class UserRepository : RepositoryBase<Users>, IUserRepository
    {
        public UserRepository(UniversityAppContext context) : base(context) { }
    }
}
