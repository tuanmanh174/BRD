using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DeMasterProCloud.Common.Infrastructure;
using DeMasterProCloud.DataAccess.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeMasterProCloud.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// Generic repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Properties
        private readonly Type _type;
        private readonly HttpContext _httpContext;
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        #endregion

        /// <summary>
        /// GenericRepository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="contextAccessor"></param>
        protected GenericRepository(AppDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
            _httpContext = contextAccessor?.HttpContext;
            _type = typeof(T);
        }

        #region Implementation
        /// <summary>
        /// Add T entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(T entity)
        {
            SetCreated(entity);
            _dbSet.Add(entity);
        }

        public void AddMany(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                SetCreated(item);
            }

            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Update T entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            SetUpdated(entity);
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateMany(IEnumerable<T> entities)
        {
            foreach (var item in entities)
            {
                SetUpdated(item);
            }

            _dbSet.AttachRange(entities);
            _dbContext.Entry(entities).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete T entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Delete a list entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Delete a list entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void DeleteRange(params T[] entities)
        {
            _dbSet.RemoveRange(entities);
        }
        /// <summary>
        /// Delete T entity by condition
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where(where).AsEnumerable();
            foreach (T obj in objects)
            {
                _dbSet.Remove(obj);
            }
        }

        /// <summary>
        /// Get T entity by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Get all entities by some condition
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        /// <summary>
        /// Get many T entity by condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> GetMany(Func<T, bool> where)
        {
            return _dbSet.Where(where);
        }

        /// <summary>
        /// Get T entity by condition
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(where);
        }

        public long Count(Expression<Func<T, bool>> expression)
        {
            return _dbSet.AsNoTracking().Count(expression);
        }

        #endregion

        #region Method to check common fields existed
        /// <summary>
        /// Check model have a specific property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected bool HasProperty(string property)
        {
            return _type.GetProperty(property) != null;
        }

        protected void SetProperty(T entity, string property, object value)
        {
            if (value != null)
            {
                entity.GetType().GetProperty(property).SetValue(entity,
                    Guid.TryParse(value.ToString(), out var number) ? number : value);
            }
        }

        protected void SetCreated(T entity)
        {
            if (HasProperty(Constants.CommonFields.CreatedBy))
            {
                var accountId = _httpContext?.User?.Claims?
                    .FirstOrDefault(m => m.Type == Constants.ClaimName.AccountId)?.Value;

                if (!string.IsNullOrEmpty(accountId))
                {
                    SetProperty(entity, Constants.CommonFields.CreatedBy, accountId);
                }
            }
            if (HasProperty(Constants.CommonFields.CreatedOn))
            {
                SetProperty(entity, Constants.CommonFields.CreatedOn, DateTime.UtcNow);
            }
        }

        protected void SetUpdated(T entity)
        {
            if (HasProperty(Constants.CommonFields.UpdatedBy))
            {
                var accountId = _httpContext?.User?.Claims?
                    .FirstOrDefault(m => m.Type == Constants.ClaimName.AccountId)?.Value;

                if (!string.IsNullOrEmpty(accountId))
                {
                    SetProperty(entity, Constants.CommonFields.UpdatedBy, accountId);
                }
            }
            if (HasProperty(Constants.CommonFields.UpdatedOn))
            {
                SetProperty(entity, Constants.CommonFields.UpdatedOn, DateTime.UtcNow);
            }
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>();
        }
        #endregion
    }
}