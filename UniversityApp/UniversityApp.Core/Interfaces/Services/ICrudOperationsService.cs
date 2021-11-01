using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UniversityApp.Core.Interfaces.Services
{
    //TODO maybe delete this if it's not needed
    public interface ICrudOperationsService<T>
    {
        Task<T> AddAsync<U>(U entity);

        Task<T> UpdateAsync<U>(U entity);

        Task DeleteAsync(Guid id);

        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null);

        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter);
    }
}
