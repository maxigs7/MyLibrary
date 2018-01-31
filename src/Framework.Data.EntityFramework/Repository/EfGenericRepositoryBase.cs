using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Common.Entities.Interfaces;
using Framework.Common.Entities.Pagination;
using Framework.Data.EntityFramework.EntityFramework;
using Framework.Data.Repository;

namespace Framework.Data.EntityFramework.Repository
{
    /// <summary>
    /// The EF-dependent, base repository for data access
    /// </summary>
    public abstract class EfGenericRepositoryBase<TEntity, TPrimaryKey> : EfBaseRepository, IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected EfGenericRepositoryBase(DbContextBase dbContext):base(dbContext)
        {
        }

        public abstract IQueryable<TEntity> GetAll();

        public Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(GetAll());
        }

        public abstract IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> whereClause,
            params Expression<Func<TEntity, object>>[] includes);

        public Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> whereClause,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(GetAll(whereClause, includes));
        }

        public abstract PagedResultList<TEntity> GetAll(PagingCriteria paging,
            params Expression<Func<TEntity, object>>[] includes);

        public Task<PagedResultList<TEntity>> GetAllAsync(PagingCriteria paging,
            params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(GetAll(paging, includes));
        }

        public abstract PagedResultList<TEntity> GetAll(PagingCriteria paging,
            Expression<Func<TEntity, bool>> whereClause, params Expression<Func<TEntity, object>>[] includes);

        public Task<PagedResultList<TEntity>> GetAllAsync(PagingCriteria paging,
            Expression<Func<TEntity, bool>> whereClause, params Expression<Func<TEntity, object>>[] includes)
        {
            return Task.FromResult(GetAll(paging, whereClause, includes));
        }

        public TEntity Get(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = GetAll();
            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }
            return entities.FirstOrDefault(CreateEqualityExpressionForId(id));
        }

        public Task<TEntity> GetAsync(TPrimaryKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = GetAll();
            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }
            return entities.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public TEntity Get(Expression<Func<TEntity, bool>> whereClause, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = GetAll();
            if (whereClause != null)
            {
                entities = entities.Where(whereClause).AsQueryable();
            }
            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            return entities.FirstOrDefault();
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> whereClause, params Expression<Func<TEntity, object>>[] includes)
        {
            var entities = GetAll();
            if (whereClause != null)
            {
                entities = entities.Where(whereClause).AsQueryable();
            }
            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            return entities.FirstOrDefaultAsync();
        }

        public abstract void Add(TEntity entity);

        public abstract void Edit(TEntity entity);

        public abstract void Delete(TEntity entity);

        public abstract void Delete(TPrimaryKey id);

        protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }

        public abstract void Dispose();
    }
}
