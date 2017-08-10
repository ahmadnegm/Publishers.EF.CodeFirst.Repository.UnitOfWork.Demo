using System;
using System.Data;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : Entity;
        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        int Save();
        int SaveInDbTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
        /// <summary>
        /// Gets the error messages which contains model's validation error
        /// </summary>
        string ErrorMessage { get; }
    }
}
