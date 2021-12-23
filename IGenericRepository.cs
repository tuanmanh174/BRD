using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeMasterProCloud.Repository
{
    /// <summary>
    /// Generic repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGenericRepository<T> where T : class
    {
        // Marks an entity as new
        void Add(T entity);

        void AddMany(IEnumerable<T> entities);

        // Marks an entity as modified
        void Update(T entity);

        void UpdateMany(IEnumerable<T> entities);

        // Marks an entity to be removed
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void DeleteRange(IEnumerable<T> entities);
        void DeleteRange(params T[] entities);
        // Get an entity by int id
        T GetById(Guid id);
        // Get an entity using delegate
        T Get(Expression<Func<T, bool>> where);
        // Gets all entities of type T
        IEnumerable<T> GetAll();
        // Gets entities using delegate
        IEnumerable<T> GetMany(Func<T, bool> where);

        long Count(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> GetListAsync(Expression<Func<T, bool>> expression);
        void SaveChanges();
        IQueryable<T> Query();
    }
}