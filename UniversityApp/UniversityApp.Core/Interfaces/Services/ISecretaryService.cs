using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;
using UniversityApp.Core.ViewModels;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ISecretaryService
    {
        Task AddAsync(SecretaryRegistrationViewModel secretaryModel, string userId);

        Task DeleteByCnpAsync(string cnp);

        Task<IQueryable<Secretaries>> GetAsync(Expression<Func<Secretaries, bool>> filter = null);

        Task UpdateAsync(Secretaries secretary);
    }
}
