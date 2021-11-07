using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UniversityApp.Core.Interfaces.Services
{
    public interface ICrudOperationsService<T> where T: class, new() 
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task DeleteAsync(Guid id);
    }
}
