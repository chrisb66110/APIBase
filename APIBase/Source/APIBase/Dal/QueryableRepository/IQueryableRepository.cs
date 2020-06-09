using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace APIBase.Dal.QueryableRepository
{
    public interface IQueryableRepository<T>
        where T : class
    {
        IQueryableRepository<T> Distinct();
        IQueryableRepository<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector) where TResult : class;
        IQueryableRepository<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IQueryableRepository<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
        IQueryableRepository<TResult> Select<TResult>(Expression<Func<T, TResult>> selector) where TResult : class;
        IQueryableRepository<T> Where(Expression<Func<T, bool>> predicate);
        IQueryableRepository<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);
        IQueryableRepository<T> Include<TPreviousProperty, TProperty>(Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath);
        IQueryableRepository<T> Intersect(IQueryableRepository<T> source2);
        IQueryableRepository<T> Union(IQueryableRepository<T> source2);
        IQueryableRepository<TResult> Join<TInner, TKey, TResult>(IQueryableRepository<TInner> inner, Expression<Func<T, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector) where TResult : class where TInner : class;
        IQueryableRepository<TOtherEntity> QueryableRepositoryOtherEntity<TOtherEntity>() where TOtherEntity : class;
    }
}
