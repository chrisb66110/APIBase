using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIBase.Dal.Repositories
{
    public abstract class BaseRepository<TEntity, TTypeId> where TEntity : class
    {
        protected readonly DbContext Context;

        protected BaseRepository(DbContext context)
        {
            Context = context;
        }

        protected async Task<List<TEntity>> _GetAllAsync()
        {
            var response = await Context.Set<TEntity>().ToListAsync();
            return response;
        }

        protected async Task<TEntity> _GetByIdAsync(TTypeId id)
        {
            var response = await Context.Set<TEntity>().FindAsync(id);
            return response;
        }

        protected async Task<TEntity> _GetByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var response = await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
            return response;
        }

        protected async Task<TEntity> _AddAsync(TEntity entity)
        {
            var response = await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return response.Entity;
        }

        protected async Task<List<TEntity>> _AddRangeAsync(List<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

        protected async Task<TEntity> _UpdateAsync(TEntity entity)
        {
            var response = Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();
            return response.Entity;
        }

        protected async Task<List<TEntity>> _UpdateAsync(List<TEntity> entities)
        {
            Context.Set<TEntity>().UpdateRange(entities);
            await Context.SaveChangesAsync();
            return entities;
        }

        protected async Task<TEntity> _RemoveAsync(TEntity entity)
        {
            var response = Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
            return response.Entity;
        }

        protected async Task<List<TEntity>> _RemoveRangeAsync(List<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
            await Context.SaveChangesAsync();
            return entities;
        }
    }
}
