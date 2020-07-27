using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APIBase.Common.AuthFunctions;
using APIBase.Common.Constants;
using APIBase.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APIBase.Dal.Repositories
{
    /*
     *To use this Repository, you need register in StartUp:
     *      builder.RegisterType<TokenFunctions>().AsImplementedInterfaces().InstancePerDependency();
     *      builder.RegisterType<HttpContextAccessor>().AsImplementedInterfaces().InstancePerDependency();
     */
    public abstract class BaseLogRepository<TContext, TEntity, TTypeId, TEntityLog>
        where TContext : DbContext
        where TEntity : BaseEntity<TTypeId>
        where TTypeId : struct, IComparable, IFormattable, IComparable<TTypeId>, IEquatable<TTypeId>
        where TEntityLog : BaseEntityLog<TEntity, TTypeId>, new()
    {
        protected readonly DbContextOptions<TContext> _options;
        protected readonly ITokenFunctions _tokenFunctions;

        protected BaseLogRepository(
            DbContextOptions<TContext> options,
            ITokenFunctions tokenFunctions)
        {
            _options = options;
            _tokenFunctions = tokenFunctions;
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
            TEntity entity,
            string methodName = null)
        {
            var userChange = _tokenFunctions.GetUsername() ?? BaseConstants.AUTH_USER_NO_LOGIN;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            var response = await context.Set<TEntity>().AddAsync(entity);

            var previousString = JsonConvert.SerializeObject(null);

            var newString = JsonConvert.SerializeObject(response.Entity);

            var log = new TEntityLog()
            {
                Username = userChange,
                DateTime = DateTime.UtcNow,
                MethodName = methodName ?? "_AddAsync",
                PreviousValue = previousString,
                NewValue = newString
            };

            await context.Set<TEntityLog>().AddAsync(log);

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return response.Entity;
        }

        protected async Task<List<TEntity>> _AddRangeAsync(
            List<TEntity> entities,
            string methodName = null)
        {
            var userChange = _tokenFunctions.GetUsername() ?? BaseConstants.AUTH_USER_NO_LOGIN;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            foreach (var entity in entities)
            {
                var result = await context.Set<TEntity>().AddAsync(entity);

                var previousString = JsonConvert.SerializeObject(null);

                var newString = JsonConvert.SerializeObject(result.Entity);

                var log = new TEntityLog
                {
                    Username = userChange,
                    DateTime = DateTime.UtcNow,
                    MethodName = methodName ?? "_AddRangeAsync",
                    PreviousValue = previousString,
                    NewValue = newString
                };

                await context.Set<TEntityLog>().AddAsync(log);
            }

            await context.SaveChangesAsync();

            await context.DisposeAsync();

            return entities;
        }

        protected async Task<TEntity> _UpdateAsync(
            TEntity entity,
            string methodName = null)
        {
            var userChange = _tokenFunctions.GetUsername() ?? BaseConstants.AUTH_USER_NO_LOGIN;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            TEntity response = null;
            var oldEntity = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));

            if (oldEntity != null)
            {
                var result = context.Set<TEntity>().Update(entity);

                var previousString = JsonConvert.SerializeObject(oldEntity);

                var newString = JsonConvert.SerializeObject(result.Entity);

                var log = new TEntityLog
                {
                    Username = userChange,
                    DateTime = DateTime.UtcNow,
                    MethodName = methodName ?? "_UpdateAsync",
                    PreviousValue = previousString,
                    NewValue = newString
                };

                await context.Set<TEntityLog>().AddAsync(log);

                await context.SaveChangesAsync();

                response = entity;
            }

            await context.DisposeAsync();

            return response;
        }

        protected async Task<List<TEntity>> _UpdateRangeAsync(
            List<TEntity> entities,
            string methodName = null)
        {
            var userChange = _tokenFunctions.GetUsername() ?? BaseConstants.AUTH_USER_NO_LOGIN;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            List<TEntity> response = null;

            var ids = entities.Select(e => e.Id);

            var oldEntities = await context.Set<TEntity>().Where(e => ids.Contains(e.Id)).AsNoTracking().ToListAsync();

            foreach (var entity in entities)
            {
                var oldEntity = oldEntities.FirstOrDefault(e => e.Id.Equals(entity.Id));
                var previousString = JsonConvert.SerializeObject(oldEntity);

                var result = context.Set<TEntity>().Update(entity);
                var newString = JsonConvert.SerializeObject(result.Entity);

                var log = new TEntityLog
                {
                    Username = userChange,
                    DateTime = DateTime.UtcNow,
                    MethodName = methodName ?? "_UpdateRangeAsync",
                    PreviousValue = previousString,
                    NewValue = newString
                };

                await context.Set<TEntityLog>().AddAsync(log);
            }

            await context.SaveChangesAsync();

            response = entities;

            await context.DisposeAsync();

            return response;
        }

        protected async Task<TEntity> _RemoveAsync(
            TEntity entity,
            string methodName = null)
        {
            var userChange = _tokenFunctions.GetUsername() ?? BaseConstants.AUTH_USER_NO_LOGIN;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            TEntity response = null;
            var oldEntity = await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));

            if (oldEntity != null)
            {
                context.Set<TEntity>().Remove(entity);

                var previousString = JsonConvert.SerializeObject(oldEntity);

                var newString = JsonConvert.SerializeObject(null);

                var log = new TEntityLog
                {
                    Username = userChange,
                    DateTime = DateTime.UtcNow,
                    MethodName = methodName ?? "_RemoveAsync",
                    PreviousValue = previousString,
                    NewValue = newString
                };

                await context.Set<TEntityLog>().AddAsync(log);

                await context.SaveChangesAsync();

                response = oldEntity;
            }

            await context.DisposeAsync();

            return response;
        }

        protected async Task<List<TEntity>> _RemoveRangeAsync(
            List<TEntity> entities,
            string methodName = null)
        {
            var userChange = _tokenFunctions.GetUsername() ?? BaseConstants.AUTH_USER_NO_LOGIN;

            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            List<TEntity> response = null;

            var ids = entities.Select(e => e.Id);

            var oldEntities = await context.Set<TEntity>().Where(e => ids.Contains(e.Id)).AsNoTracking().ToListAsync();

            if (oldEntities.Count == entities.Count)
            {
                context.Set<TEntity>().RemoveRange(entities);

                foreach (var entity in entities)
                {
                    var oldEntity = oldEntities.First(e => e.Id.Equals(entity.Id));

                    var previousString = JsonConvert.SerializeObject(oldEntity);

                    var newString = JsonConvert.SerializeObject(null);

                    var log = new TEntityLog
                    {
                        Username = userChange,
                        DateTime = DateTime.UtcNow,
                        MethodName = methodName ?? "_RemoveRangeAsync",
                        PreviousValue = previousString,
                        NewValue = newString
                    };

                    await context.Set<TEntityLog>().AddAsync(log);
                }

                await context.SaveChangesAsync();

                response = oldEntities;
            }

            await context.DisposeAsync();

            return response;
        }

        protected async Task<List<TEntityLog>> _GetLogsAsync(DateTime? from = default, DateTime? to = default)
        {
            var context = (TContext)Activator.CreateInstance(typeof(TContext), _options);

            List<TEntityLog> response;

            if (from != default && to != default)
            {
                response = await context.Set<TEntityLog>().Where(l => from < l.DateTime && l.DateTime > to).AsNoTracking().ToListAsync();
            }
            else
            {
                response = await context.Set<TEntityLog>().AsNoTracking().ToListAsync();
            }

            await context.DisposeAsync();

            foreach (var each in response)
            {
                each.PreviousEntity = JsonConvert.DeserializeObject<TEntity>(each.PreviousValue);
                each.NewEntity = JsonConvert.DeserializeObject<TEntity>(each.NewValue);
            }

            return response;
        }
    }
}
