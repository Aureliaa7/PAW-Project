using UniversityApp.Core.DomainEntities;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces;

namespace UniversityApp.Repositories
{
    //TODO delete it
    public class SecretaryRepository : RepositoryBase<Secretaries>, ISecretaryRepository
    {
        public SecretaryRepository(UniversityAppContext context) : base(context) { }
    }
}
