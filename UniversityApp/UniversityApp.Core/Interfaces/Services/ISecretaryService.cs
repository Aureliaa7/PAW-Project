using Microsoft.AspNetCore.Http;
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
        Task AddAsync(SecretaryRegistrationViewModel secretaryModel, IFormFile image = null);

        Task DeleteByCnpAsync(string cnp);

        Task<IQueryable<Secretary>> GetAsync(Expression<Func<Secretary, bool>> filter = null);

        Task UpdateAsync(Secretary secretary);
    }
}
