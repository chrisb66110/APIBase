using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APIBase.Dal.QueryableRepository;
using Microsoft.EntityFrameworkCore;

namespace APIBase.Dal.Repositories
{
    /*
     * To use this repository, you need register the generic repository in start up, you can add:
     *          builder.RegisterGeneric(typeof(BaseAbstractRepository<,,>)).As(typeof(IBaseAbstractRepository<,,>));
     *
     * To use in Bll, you need add dependency
     *          IBaseAbstractRepository<model, modelDto, yourContext> modelRepository
     */
    public interface IBaseAbstractRepository<TEntity, TDto, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        public Task<List<TDto>> GetAsync(Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null);
        public Task<List<TResultGroupBy>> GetAsync<TResultGroupBy>(Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class;
        public Task<TDto> GetFirstOrDefaultAsync(Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null);
        public Task<TResultGroupBy> GetFirstOrDefaultAsync<TResultGroupBy>(Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class;
        public Task<TDto> GetLastOrDefaultAsync(Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction);
        public Task<TResultGroupBy> GetLastOrDefaultAsync<TResultGroupBy>(Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class;
        public Task<TDto> AddAsync(TDto dto);
        public Task<List<TDto>> AddRangeAsync(List<TDto> dtos);
        public Task<TDto> UpdateAsync(TDto dto);
        public Task<List<TDto>> UpdateRangeAsync(List<TDto> dto);
        public Task<TDto> RemoveAsync(TDto dto);
        public Task<List<TDto>> RemoveRangeAsync(List<TDto> dtos);
        public Task<TDto> UpsertAsync(TDto dto, Expression<Func<TEntity, object>> on, Expression<Func<TEntity, TEntity, TEntity>> whenMatched);
        public Task<List<TDto>> UpsertRangeAsync(List<TDto> dto, Expression<Func<TEntity, object>> on, Expression<Func<TEntity, TEntity, TEntity>> whenMatched);
        public Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null);
        public Task<TResult> MinAsync<TResult, TResultGroupBy>(Expression<Func<TResultGroupBy, TResult>> selector, Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class;
        public Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null);
        public Task<TResult> MaxAsync<TResult, TResultGroupBy>(Expression<Func<TResultGroupBy, TResult>> selector, Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class;
        public Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector, Func<IQueryableRepository<TEntity>, IQueryableRepository<TEntity>> filterFunction = null);
        public Task<decimal> SumAsync<TResultGroupBy>(Expression<Func<TResultGroupBy, decimal>> selector, Func<IQueryableRepository<TEntity>, IQueryableRepository<TResultGroupBy>> filterFunction) where TResultGroupBy : class;
    }
}
