using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APIBase.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBase.Dal.Repositories
{
    public abstract class BaseRepository<TContext, TEntity, TTypeId>
        where TContext : DbContext
        where TEntity : BaseEntity<TTypeId>
        where TTypeId : struct, IComparable, IFormattable, IComparable<TTypeId>, IEquatable<TTypeId>
    {
        protected readonly DbContextOptions<TContext> _options;

        protected BaseRepository(
            DbContextOptions<TContext> options)
        {
            _options = options;
        }

        protected async Task<List<TEntity>> _GetAllAsync()
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var response = await context.Set<TEntity>().AsNoTracking().ToListAsync();

            await context.DisposeAsync();

            return response;
        }

        protected async Task<TEntity> _GetByIdAsync(
            TTypeId id)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var response = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));

            await context.DisposeAsync();

            return response;
        }

        protected async Task<TEntity> _GetByAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var response = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);

            await context.DisposeAsync();

            return response;
        }

        protected async Task<TEntity> _AddAsync(
            TEntity entity)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var response = await context.Set<TEntity>().AddAsync(entity);
            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return response.Entity;
        }

        protected async Task<List<TEntity>> _AddRangeAsync(
            List<TEntity> entities)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            await context.Set<TEntity>().AddRangeAsync(entities);
            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return entities;
        }

        protected async Task<TEntity> _UpdateAsync(
            TEntity entity)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            TEntity response = null;
            var oldEntity = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));

            if (oldEntity != null)
            {
                context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync();

                response = entity;
            }

            await context.DisposeAsync();

            return response;
        }

        protected async Task<List<TEntity>> _UpdateRangeAsync(
            List<TEntity> entities)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            List<TEntity> response = null;

            var ids = entities.Select(e => e.Id);

            var oldEntities = await context.Set<TEntity>().Where(e => ids.Contains(e.Id)).AsNoTracking().ToListAsync();

            if (oldEntities.Count == entities.Count)
            {
                context.Set<TEntity>().UpdateRange(entities);
                await context.SaveChangesAsync();

                response = entities;
            }

            await context.DisposeAsync();

            return response;
        }

        protected async Task<TEntity> _RemoveAsync(
            TEntity entity)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            TEntity response = null;
            var oldEntity = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));

            if (oldEntity != null)
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();

                response = oldEntity;
            }

            await context.DisposeAsync();

            return response;
        }

        protected async Task<List<TEntity>> _RemoveRangeAsync(
            List<TEntity> entities)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            List<TEntity> response = null;

            var ids = entities.Select(e => e.Id);

            var oldEntities = await context.Set<TEntity>().Where(e => ids.Contains(e.Id)).AsNoTracking().ToListAsync();

            if (oldEntities.Count == entities.Count)
            {
                context.Set<TEntity>().RemoveRange(entities);
                await context.SaveChangesAsync();

                response = oldEntities;
            }

            await context.DisposeAsync();

            return response;
        }
    }
}
