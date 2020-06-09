using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APIBase.Dal.QueryableRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace APIBase.Dal.Repositories
{
    public class BaseAbstractRepository<TEntity, TDto, TContext> : IBaseAbstractRepository<TEntity, TDto, TContext>, IDisposable
        where TEntity : class
        where TContext : DbContext
    {
        private readonly DbContextOptions<TContext> _options;
        private readonly IMapper _mapper;

        public BaseAbstractRepository(
            DbContextOptions<TContext> options,
            IMapper mapper)
        {
            _options = options;
            _mapper = mapper;
        }

        /*
         * Return the same queryable, for call when don't use filter
         */
        private IQueryableRepository<TEntity> DefaultFilterFunction(IQueryableRepository<TEntity> queryable)
        {
            return queryable;
        }

        /*
         * Get with optional filters
         */
        public async Task<List<TDto>> GetAsync(
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null)
        {
            if (filterFunction == null)
            {
                filterFunction = DefaultFilterFunction;
            }

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            queryableRepository = filterFunction(queryableRepository);

            var entities = await ((QueryableRepository<TEntity, TContext>)queryableRepository).AsQueryable().AsNoTracking().ToListAsync();

            var response = _mapper.Map<List<TEntity>, List<TDto>>(entities);

            await context.DisposeAsync();

            return response;
        }

        /*
         * Get with optional filters and use Group by
         */
        public async Task<List<TResultGroupBy>> GetAsync<TResultGroupBy>(
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction)
            where TResultGroupBy : class
        {

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            var groupQueryable = filterFunction(queryableRepository);

            var results = await ((QueryableRepository<TResultGroupBy, TContext>)groupQueryable).AsQueryable().ToListAsync();

            await context.DisposeAsync();

            return results;
        }

        /*
         * Get first or default with optional filter
         */
        public async Task<TDto> GetFirstOrDefaultAsync(
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null)
        {
            if (filterFunction == null)
            {
                filterFunction = DefaultFilterFunction;
            }

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            queryableRepository = filterFunction(queryableRepository);

            var entity = await ((QueryableRepository<TEntity, TContext>)queryableRepository).AsQueryable().AsNoTracking().FirstOrDefaultAsync();

            var response = _mapper.Map<TEntity, TDto>(entity);

            await context.DisposeAsync();

            return response;
        }

        /*
         * Get first or default with optional filter with group by
         */
        public async Task<TResultGroupBy> GetFirstOrDefaultAsync<TResultGroupBy>(
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class
        {

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            var groupQueryable = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TResultGroupBy, TContext>)groupQueryable).AsQueryable().AsNoTracking().FirstOrDefaultAsync();

            await context.DisposeAsync();

            return result;
        }

        /*
         * Get last or default with optional filter
         */
        public async Task<TDto> GetLastOrDefaultAsync(
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction)
        {
            if (filterFunction == null)
            {
                filterFunction = DefaultFilterFunction;
            }

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            queryableRepository = filterFunction(queryableRepository);

            var entity = await ((QueryableRepository<TEntity, TContext>)queryableRepository).AsQueryable().AsNoTracking().LastOrDefaultAsync();

            var response = _mapper.Map<TEntity, TDto>(entity);

            await context.DisposeAsync();

            return response;
        }

        /*
         * Get last or default with optional filter
         */
        public async Task<TResultGroupBy> GetLastOrDefaultAsync<TResultGroupBy>(
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            var groupQueryable = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TResultGroupBy, TContext>)groupQueryable).AsQueryable().AsNoTracking().LastOrDefaultAsync();

            await context.DisposeAsync();

            return result;
        }

        /*
         * Get first or default with optional filter and remove entity returned
         */
        public async Task<TDto> GetFirstOrDefaultAndRemoveAsync(
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null)
        {
            if (filterFunction == null)
            {
                filterFunction = DefaultFilterFunction;
            }

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            queryableRepository = filterFunction(queryableRepository);

            var entity = await ((QueryableRepository<TEntity, TContext>)queryableRepository).AsQueryable().AsNoTracking().FirstOrDefaultAsync();

            if (entity != null)
            {
                context.Remove(entity);
                await context.SaveChangesAsync();
            }

            var response = _mapper.Map<TEntity, TDto>(entity);

            await context.DisposeAsync();

            return response;
        }

        /*
         * Add entity
         */
        public async Task<TDto> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            await context.Set<TEntity>().AddAsync(entity);

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return dto;
        }

        /*
         * Add entities
         */
        public async Task<List<TDto>> AddRangeAsync(List<TDto> dtos)
        {
            var entities = _mapper.Map<List<TDto>, List<TEntity>>(dtos);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            await context.Set<TEntity>().AddRangeAsync(entities);

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return dtos;
        }

        /*
         * Update entity
         * Need the Id in Dto to update info
         * Don't change Id
         */
        public async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            context.Set<TEntity>().Update(entity);

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return dto;
        }

        /*
         * Update entities
         * Need the Id in Dtos to update info
         * Don't change Id
         */
        public async Task<List<TDto>> UpdateRangeAsync(List<TDto> dto)
        {
            var entities = _mapper.Map<List<TDto>, List<TEntity>>(dto);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            context.Set<TEntity>().UpdateRange(entities);

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return dto;
        }

        /*
         * Delete entity
         * Need the Id in Dto to remove entity
         */
        public async Task<TDto> RemoveAsync(TDto dto)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            context.Set<TEntity>().Remove(entity);

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return dto;
        }

        /*
         * Delete entities
         * Need the Id in Dtos to remove entity
         */
        public async Task<List<TDto>> RemoveRangeAsync(List<TDto> dtos)
        {
            var entities = _mapper.Map<List<TDto>, List<TEntity>>(dtos);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            context.Set<TEntity>().RemoveRange(entities);

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return dtos;
        }

        /*
         * Upsert entity
         */
        public async Task<TDto> UpsertAsync(TDto dto, Expression<Func<TEntity, object>> on, Expression<Func<TEntity, TEntity, TEntity>> whenMatched)
        {
            var entity = _mapper.Map<TDto, TEntity>(dto);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            await context.Set<TEntity>().Upsert(entity).On(on).WhenMatched(whenMatched).RunAsync();

            await context.DisposeAsync();

            return dto;
        }

        /*
         * Upsert entities
         * Need the Id in Dtos to remove entity
         */
        public async Task<List<TDto>> UpsertRangeAsync(List<TDto> dtos, Expression<Func<TEntity, object>> on, Expression<Func<TEntity, TEntity, TEntity>> whenMatched)
        {
            var entities = _mapper.Map<List<TDto>, List<TEntity>>(dtos);

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            await context.Set<TEntity>().UpsertRange(entities).On(on).WhenMatched(whenMatched).RunAsync();

            await context.DisposeAsync();

            return dtos;
        }
        
        /*
         * Get Min Entity by selector with optional filters
         */
        public async Task<TResult> MinAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null)
        {
            if (filterFunction == null)
            {
                filterFunction = DefaultFilterFunction;
            }

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            queryableRepository = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TEntity, TContext>)queryableRepository).AsQueryable().MinAsync(selector);

            await context.DisposeAsync();

            return result;
        }

        /*
         * Get Min Entity by selector with optional filters per group by
         */
        public async Task<TResult> MinAsync<TResult, TResultGroupBy>(
            Expression<Func<TResultGroupBy, TResult>> selector,
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction)
            where TResultGroupBy : class
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            var groupQueryable = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TResultGroupBy, TContext>)groupQueryable).AsQueryable().MinAsync(selector);

            await context.DisposeAsync();

            return result;
        }

        /*
         * Get Max Entity by selector with optional filters
         */
        public async Task<TResult> MaxAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null)
        {
            if (filterFunction == null)
            {
                filterFunction = DefaultFilterFunction;
            }

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            queryableRepository = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TEntity, TContext>)queryableRepository).AsQueryable().MaxAsync(selector);

            await context.DisposeAsync();

            return result;
        }

        /*
         * Get Max Entity by selector with optional filters per group by
         */
        public async Task<TResult> MaxAsync<TResult, TResultGroupBy>(
            Expression<Func<TResultGroupBy, TResult>> selector,
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction)
            where TResultGroupBy : class
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            var groupQueryable = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TResultGroupBy, TContext>)groupQueryable).AsQueryable().MaxAsync(selector);

            await context.DisposeAsync();

            return result;
        }

        /*
         * Get Sum Entity by selector with optional filters
         */
        public async Task<decimal> SumAsync(
            Expression<Func<TEntity, decimal>> selector,
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null)
        {
            if (filterFunction == null)
            {
                filterFunction = DefaultFilterFunction;
            }

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            queryableRepository = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TEntity, TContext>)queryableRepository).AsQueryable().SumAsync(selector);

            await context.DisposeAsync();

            return result;
        }

        /*
         * Get Sum Entity by selector with optional filters
         */
        public async Task<decimal> SumAsync<TResultGroupBy>(
            Expression<Func<TResultGroupBy, decimal>> selector,
            Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction)
            where TResultGroupBy : class
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var queryable = context.Set<TEntity>().AsQueryable();

            var queryableRepository = QueryableRepository<TEntity, TContext>.AsQueryableRepository(queryable, context);

            var groupQueryable = filterFunction(queryableRepository);

            var result = await ((QueryableRepository<TResultGroupBy, TContext>)groupQueryable).AsQueryable().SumAsync(selector);

            await context.DisposeAsync();

            return result;
        }

        public void Dispose()
        {
        }
    }
}
