using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace APIBase.Dal.Repositories
{
    public abstract class BaseRepository<TEntity, TTypeId> where TEntity : class
    {
        private readonly DbContext _context;

        protected BaseRepository(DbContext context)
        {
            _context = context;
        }

        protected async Task<List<TEntity>> _GetAllAsync()
        {
            var response = await _context.Set<TEntity>().ToListAsync();
            return response;
        }

        protected async Task<TEntity> _GetByIdAsync(TTypeId id)
        {
            var response = await _context.Set<TEntity>().FindAsync(id);
            return response;
        }

        protected async Task<TEntity> _AddAsync(TEntity entity)
        {
            var response = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        protected async Task<List<TEntity>> _AddRangeAsync(List<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        protected async Task<TEntity> _UpdateAsync(TEntity entity)
        {
            var response = _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        protected async Task<List<TEntity>> _UpdateAsync(List<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        protected async Task<TEntity> _RemoveAsync(TEntity entity)
        {
            var response = _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        protected async Task<List<TEntity>> _RemoveRangeAsync(List<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
    }
}
