using System.Linq;
using APIBase.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace APIBase.Dal.Extensions
{
    public static class QueryableIncludeExtensions
    {
        public static IQueryable<TEntity> IncludeAll<TEntity>(
            this IQueryable<TEntity> query)
            where TEntity : class
        {
            foreach (var prop in typeof(TEntity).GetProperties())
            {
                var name = prop.Name;

                var propType = prop.PropertyType;

                var isPrimitiveType = propType.IsPrimitiveType();

                if (!isPrimitiveType)
                {
                    query = query.Include(name);
                }
            }

            return query;
        }
    }
}
