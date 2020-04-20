using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIBase.DAL.Contexts
{
    [ExcludeFromCodeCoverage]
    public class BaseContext
    {
        private readonly DbContextOptions _options;

        public BaseContext(DbContextOptions options)
        {
            _options = options;
        }

        protected async Task<bool> PendingMigrations()
        {
            bool response;

            await using (var context = new DbContext(_options))
            {
                response = context.Database.GetPendingMigrations().AsQueryable().Any();
            }

            return response;
        }

        protected async Task ApplyMigrations()
        {
            await using (var context = new DbContext(_options))
            {
                await context.Database.MigrateAsync();
            }
        }

        protected async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        {
            List<TEntity> response;

            await using (var context = new DbContext(_options))
            {
                response = await context.Set<TEntity>().ToListAsync();
            }

            return response;
        }

        protected async Task<TEntity> GetByIdAsync<TEntity, TTypeId>(TTypeId id) where TEntity : class
        {
            TEntity response;

            await using (var context = new DbContext(_options))
            {
                response = await context.Set<TEntity>().FindAsync(id);
            }

            return response;
        }

        protected async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await using (var context = new DbContext(_options))
            {
                await context.Set<TEntity>().AddAsync(entity);
                await context.SaveChangesAsync();
            }

            return entity;
        }

        protected async Task<List<TEntity>> AddRangeAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            await using (var context = new DbContext(_options))
            {
                await context.Set<TEntity>().AddRangeAsync(entities);
                await context.SaveChangesAsync();
            }

            return entities;
        }

        protected async Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await using (var context = new DbContext(_options))
            {
                context.Set<TEntity>().Update(entity);
                await context.SaveChangesAsync();
            }

            return entity;
        }

        protected async Task<List<TEntity>> UpdateAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            await using (var context = new DbContext(_options))
            {
                context.Set<TEntity>().UpdateRange(entities);
                await context.SaveChangesAsync();
            }

            return entities;
        }

        protected async Task<TEntity> RemoveAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await using (var context = new DbContext(_options))
            {
                context.Set<TEntity>().Remove(entity);
                await context.SaveChangesAsync();
            }

            return entity;
        }

        protected async Task<List<TEntity>> RemoveRangeAsync<TEntity>(List<TEntity> entities) where TEntity : class
        {
            await using (var context = new DbContext(_options))
            {
                context.Set<TEntity>().RemoveRange(entities);
                await context.SaveChangesAsync();
            }

            return entities;
        }
    }
}
