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
        User CreateUser(List<IFormFile> image, string userName, string email, string phoneNumber);

        Task<IEnumerable<User>> GetAsync(Expression<Func<User, bool>> filter = null);

        Task DeleteAsync(Guid id);
    }
}
