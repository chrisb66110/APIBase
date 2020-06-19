using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace APIBase.Dal.QueryableRepository
{
    public class QueryableRepository<T, TContext> : IQueryableRepository<T>
        where T : class
        where TContext : DbContext
    {
        private IQueryable<T> _source;

        private bool _isIncludeQueryable;

        private readonly TContext _context;

        private QueryableRepository(
            IQueryable<T> source,
            TContext context)
        {
            _source = source;
            _context = context;
            _isIncludeQueryable = false;
        }

        public static IQueryableRepository<T> AsQueryableRepository(
            IQueryable<T> source,
            TContext context)
        {
            var newInstance = new QueryableRepository<T, TContext>(source, context);
            return newInstance;
        }

        public IQueryable<T> AsQueryable()
        {
            return _source;
        }

        public IQueryableRepository<T> Where(
            Expression<Func<T, bool>> predicate)
        {
            _source = _source.Where(predicate);

            _isIncludeQueryable = false;

            return this;
        }

        public IQueryableRepository<TResult> Select<TResult>(
            Expression<Func<T, TResult>> selector)
            where TResult : class
        {
            var result = _source.Select(selector);

            _isIncludeQueryable = false;

            var newInstance = new QueryableRepository<TResult, TContext>(result, _context);

            return newInstance;
        }

        public IQueryableRepository<T> OrderBy<TKey>(
            Expression<Func<T, TKey>> keySelector)
        {
            _source = _source.OrderBy(keySelector);

            _isIncludeQueryable = false;

            return this;
        }

        public IQueryableRepository<T> OrderByDescending<TKey>(
            Expression<Func<T, TKey>> keySelector)
        {
            _source = _source.OrderByDescending(keySelector);

            _isIncludeQueryable = false;

            return this;
        }


        public IQueryableRepository<TResult> GroupBy<TKey, TResult>(
            Expression<Func<T, TKey>> keySelector,
            Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector)
            where TResult : class
        {
            var result = _source.GroupBy(keySelector, resultSelector);

            _isIncludeQueryable = false;

            var newInstance = new QueryableRepository<TResult, TContext>(result, _context);

            return newInstance;
        }

        public IQueryableRepository<T> Distinct()
        {
            _source = _source.Distinct();

            _isIncludeQueryable = false;

            return this;
        }

        public IQueryableRepository<T> Include<TProperty>(
            Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            _source = _source.Include(navigationPropertyPath);

            _isIncludeQueryable = true;

            return this;
        }

        public IQueryableRepository<T> Include<TPreviousProperty, TProperty>(
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
        {
            if (!_isIncludeQueryable)
            {
                throw new Exception("You can use this method after use Include method");
            }

            var include = (IIncludableQueryable<T, TPreviousProperty>)_source;

            _source = include.ThenInclude(navigationPropertyPath);

            _isIncludeQueryable = true;

            return this;
        }

        public IQueryableRepository<T> Intersect(
            [NotNull]IQueryableRepository<T> other)
        {
            var sourceOther = ((QueryableRepository<T, TContext>)other).AsQueryable();

            _source = _source.Intersect(sourceOther);

            _isIncludeQueryable = false;

            return this;
        }

        public IQueryableRepository<T> Union(
            IQueryableRepository<T> other)
        {
            var sourceOther = ((QueryableRepository<T, TContext>)other).AsQueryable();

            _source = _source.Union(sourceOther);

            _isIncludeQueryable = false;

            return this;
        }

        public IQueryableRepository<TResult> Join<TInner, TKey, TResult>(
            IQueryableRepository<TInner> inner,
            Expression<Func<T, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<T, TInner, TResult>> resultSelector)
            where TResult : class
            where TInner : class
        {
            var sourceOther = ((QueryableRepository<TInner, TContext>)inner).AsQueryable();

            var result = _source.Join(sourceOther, outerKeySelector, innerKeySelector, resultSelector);

            _isIncludeQueryable = false;

            var newInstance = new QueryableRepository<TResult, TContext>(result, _context);

            return newInstance;
        }

        public IQueryableRepository<TOtherEntity> QueryableRepositoryOtherEntity<TOtherEntity>()
            where TOtherEntity : class
        {
            var queryable = _context.Set<TOtherEntity>().AsQueryable();

            _isIncludeQueryable = false;

            var newInstance = new QueryableRepository<TOtherEntity, TContext>(queryable, _context);

            return newInstance;
        }
    }
}
