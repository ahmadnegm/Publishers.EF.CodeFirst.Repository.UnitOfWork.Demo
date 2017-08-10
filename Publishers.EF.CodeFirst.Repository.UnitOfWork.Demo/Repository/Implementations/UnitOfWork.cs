using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main.Interfaces;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Repository.Interfaces;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Repository.Implementations
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        public string ErrorMessage { get; private set; }
        private readonly TContext _context;
        private bool _disposed;
        private Dictionary<string, object> _repositories;

        public UnitOfWork()
        {
            ErrorMessage = null;
            _context = Activator.CreateInstance<TContext>();
            _repositories = new Dictionary<string, object>();
        }

        public UnitOfWork(TContext context)
        {
            _context = context;
            ErrorMessage = null;
        }

        public IRepository<TSet> GetRepository<TSet>() where TSet : Entity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<string, object>();
            }

            if (_repositories.ContainsKey(typeof(TSet).Name))
            {
                return _repositories[typeof(TSet).Name] as IRepository<TSet>;
            }

            var repositoryInstance = new Repository<TSet, TContext>(_context);
            _repositories.Add(typeof(TSet).Name, repositoryInstance);
            return repositoryInstance;
        }

        public int Save()
        {
            try
            {
                #region Handling auditing

                var modifiedEntries = _context.ChangeTracker.Entries()
                    .Where(x => x.Entity is IAuditableEntity
                                && (x.State == EntityState.Added ||
                                    x.State == EntityState.Modified));

                foreach (var entry in modifiedEntries)
                {
                    var entity = entry.Entity as IAuditableEntity;
                    if (entity != null)
                    {
                        var identityName = Thread.CurrentPrincipal.Identity.Name;
                        var now = DateTime.UtcNow;

                        if (entry.State == EntityState.Added)
                        {
                            entity.CreatedBy = identityName;
                            entity.Created = now;
                        }
                        else
                        {
                            _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                            _context.Entry(entity).Property(x => x.Created).IsModified = false;
                        }

                        entity.ModifiedBy = identityName;
                        entity.Modified = now;
                    }
                } 
                #endregion

                var affectedRows = _context.SaveChanges();
                return affectedRows;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(
                    validationErrors => validationErrors.ValidationErrors))
                {
                    ErrorMessage += $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" +
                                    Environment.NewLine;
                }
                throw new Exception(ErrorMessage, dbEx);
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                throw new Exception(ErrorMessage, exception);
            }
        }

        public int SaveInDbTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            DbContextTransaction transaction = null; 
            try
            {
                transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
                using (transaction)
                {
                    #region Handling auditing

                    var modifiedEntries = _context.ChangeTracker.Entries()
                        .Where(x => x.Entity is IAuditableEntity
                                    && (x.State == EntityState.Added ||
                                        x.State == EntityState.Modified));

                    foreach (var entry in modifiedEntries)
                    {
                        var entity = entry.Entity as IAuditableEntity;
                        if (entity != null)
                        {
                            var identityName = Thread.CurrentPrincipal.Identity.Name;
                            var now = DateTime.UtcNow;

                            if (entry.State == EntityState.Added)
                            {
                                entity.CreatedBy = identityName;
                                entity.Created = now;
                            }
                            else
                            {
                                _context.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                                _context.Entry(entity).Property(x => x.Created).IsModified = false;
                            }

                            entity.ModifiedBy = identityName;
                            entity.Modified = now;
                        }
                    }

                    #endregion

                    var affectedRows = _context.SaveChanges();
                    transaction.Commit();
                    return affectedRows;
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(
                    validationErrors => validationErrors.ValidationErrors))
                {
                    ErrorMessage += $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}" +
                                    Environment.NewLine;
                }
                transaction?.Rollback();
                throw new Exception(ErrorMessage, dbEx);
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                transaction?.Rollback();
                throw new Exception(ErrorMessage, exception);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
    }
}
