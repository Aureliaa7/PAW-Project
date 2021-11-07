using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ISecretaryService
    {
        Task AddAsync(Secretary secretary, string password);

        Task DeleteByCnpAsync(string cnp);

        Task<IEnumerable<Secretary>> GetAllAsync(Expression<Func<Secretary, bool>> filter = null);

        Task UpdateAsync(Secretary secretary);

        Task<Secretary> GetFirstOrDefaultAsync(Expression<Func<Secretary, bool>> filter);

        Task DeleteAsync(Guid id);
    }
}
