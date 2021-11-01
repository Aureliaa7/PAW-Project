using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Core.DomainEntities;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface IUserService
    {
        Users CreateUser(List<IFormFile> image, string userName, string email, string phoneNumber);

        Task<IEnumerable<Users>> GetAsync(Expression<Func<Users, bool>> filter = null);

        Task DeleteAsync(Guid id);
    }
}
