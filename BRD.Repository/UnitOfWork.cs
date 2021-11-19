using BRD.DataAccess.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BRD.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        #region inject field variables
        private readonly AppDbContext _appDbContext;
        #endregion

        #region data members

        private readonly IHttpContextAccessor _httpContext;
        //private ISubjectCategoryRepository _subjectCategoryRepository;
       
        #endregion

        /// <summary>
        /// Unit of work constructor
        /// </summary>
        /// <param name="appDbContext"></param>
        /// <param name="contextAccessor"></param>
        public UnitOfWork(AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContext = contextAccessor;
        }

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        #region Properties
        /// <summary>
        /// Get AppDbContext
        /// </summary>
        public AppDbContext AppDbContext => _appDbContext;
        /// <summary>
        /// Get UserRepository
        /// </summary>

        /// <summary>
        /// Get UserLoginRepository
        /// </summary>

        //public ISubjectCategoryRepository SubjectCategoryRepository
        //{
        //    get
        //    {
        //        return _subjectCategoryRepository = _subjectCategoryRepository ?? new SubjectCategoryRepository(_appDbContext, _httpContext);
        //    }
        //}

        

        /// <summary>
        /// Save
        /// </summary>
        public void Save()
        {
            _appDbContext.SaveChanges();
        }
        /// <summary>
        /// Save Async
        /// </summary>
        public async Task SaveAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
        #endregion

        #region dispose
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _appDbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _appDbContext.Database.BeginTransactionAsync();
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _appDbContext.Database.CreateExecutionStrategy();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _appDbContext.Database.BeginTransaction();
        }
        #endregion
    }
}
