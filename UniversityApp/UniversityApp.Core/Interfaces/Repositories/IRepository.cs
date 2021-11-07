using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UniversityApp.Interfaces
{
    public interface IRepository<T>
    {
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> expression = null);

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<T> DeleteAsync(Guid id);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);
    }
}
