using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Repository.Interfaces
{
    /// <summary>
    /// Repository Interface defines the base
    /// functionality required by all Repositories.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type that requires a Repository. </typeparam>
    public interface IRepository<TEntity> : IDisposable
    {
        /// <summary>
        /// Gets all objects from database
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IQueryable<TEntity> All(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");

        /// <summary>
        /// Gets objects from database based on T-SQL query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);

        /// <summary>
        /// Gets objects from database by filter.
        /// </summary>
        /// <param name="predicate">Specified a filter</param>
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets objects from database with filtrating and paging.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="filter">Specified a filter</param>
        /// <param name="total">Returns the total records count of the filter.</param>
        /// <param name="index">Specified the page index.</param>
        /// <param name="size">Specified the page size</param>
        IQueryable<TEntity> Filter<TKey>(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50);

        /// <summary>
        /// Gets the object(s) is exists in database by specified filter.
        /// </summary>
        /// <param name="predicate">Specified the filter expression</param>
        bool Contains(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        TEntity Find(params object[] keys);

        /// <summary>
        /// Find object by specified expression.
        /// </summary>
        /// <param name="predicate"></param>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Create a new object to database.
        /// </summary>
        /// <param name="entity">Specified a new object to create.</param>
        void Create(TEntity entity);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="entityId">Specified an ID of an object to delete.</param>
        void Delete(object entityId);

        /// <summary>
        /// Delete the object from database.
        /// </summary>
        /// <param name="entity">Specified an existing object to delete.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Update object changes and save to database.
        /// </summary>
        /// <param name="entity">Specified the object to save.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Get the total objects count.
        /// </summary>
        int Count { get; }
    }
}
