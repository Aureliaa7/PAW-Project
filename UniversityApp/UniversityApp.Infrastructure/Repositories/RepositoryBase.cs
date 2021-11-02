using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UniversityApp.Infrastructure.AppDbContext;
using UniversityApp.Interfaces;

namespace UniversityApp.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected UniversityAppContext Context { get; set; }

        public RepositoryBase(UniversityAppContext context)
        {
            Context = context;
        }

        public Task<IQueryable<T>> FindAsync(Expression<Func<T, bool>> expression = null)
        {
            if (expression != null)
            {
                return Task.FromResult(Context.Set<T>().Where(expression).AsNoTracking());
            }
            return Task.FromResult(Context.Set<T>().AsNoTracking());
        }

        public Task<T> CreateAsync(T entity)
        {
            Context.Set<T>().Add(entity);
            return Task.FromResult(entity);
        }

        public Task<T> UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);

            return Task.FromResult(entity);
        }

        //TODO remove this one
        public Task<T> DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Task.FromResult(entity);
        }

        public async Task<T> DeleteAsync(Guid id)
        {
            var entityToBeDeleted = await Context.Set<T>().FindAsync(id);
            if (entityToBeDeleted == null)
            {
                return entityToBeDeleted;
            }
            Context.Set<T>().Remove(entityToBeDeleted);

            return entityToBeDeleted;
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
        {
            var entities = Context.Set<T>().Where(filter);

            return Task.FromResult(entities.Any());
        }
    }
}
