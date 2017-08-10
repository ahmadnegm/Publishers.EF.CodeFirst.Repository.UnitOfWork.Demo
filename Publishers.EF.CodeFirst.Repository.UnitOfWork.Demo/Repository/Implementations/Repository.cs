using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Entities.Main;
using Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Repository.Interfaces;

namespace Publishers.EF.CodeFirst.Repository.UnitOfWork.Demo.Repository.Implementations
{
    public class Repository<TEntity, TContext> : IRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        private readonly TContext _context;
        protected DbSet<TEntity> DbSet => _context.Set<TEntity>();

        public Repository(TContext session)
        {
            _context = session;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public IQueryable<TEntity> All(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            //return DbSet.AsQueryable();
            var query = DbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy?.Invoke(query).AsQueryable() ?? query.AsQueryable();
        }

        public IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        {
            return DbSet.SqlQuery(query, parameters).ToList();
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();
        }

        public IQueryable<TEntity> Filter<TKey>(Expression<Func<TEntity, bool>> predicate, out int total, int index = 0,
            int size = 50)
        {
            var result = DbSet.Where(predicate);
            total = result.Count();
            return result.Skip(index).Take(size);
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Count(predicate) > 0;
        }

        public TEntity Find(params object[] keys)
        {
            return DbSet.Find(keys);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        public void Create(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(object entityId)
        {
            var entity = DbSet.Find(entityId);
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
                DbSet.Remove(obj);
        }

        public void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            DbSet.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public int Count => DbSet.Count();

    }

}
