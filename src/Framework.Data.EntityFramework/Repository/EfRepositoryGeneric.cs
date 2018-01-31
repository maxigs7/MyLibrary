using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Framework.Common.Entities.Interfaces;
using Framework.Common.Entities.Pagination;
using Framework.Data.EntityFramework.EntityFramework;
using Framework.Data.Helpers;

namespace Framework.Data.EntityFramework.Repository
{
    /// <summary>
    /// Implements IRepository for Entity Framework.
    /// </summary>
    /// <typeparam name="TDbContext">DbContext which contains <typeparamref name="TEntity"/>.</typeparam>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public class EfRepository<TEntity, TPrimaryKey> : EfGenericRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table => Context.Set<TEntity>();


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public EfRepository(DbContextBase dbContext) : base(dbContext)
        {
        }


        public override IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public override IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> whereClause,params Expression<Func<TEntity, object>>[] includes)
        {
            var dbset = GetAll();
            if (whereClause != null)
            {
                dbset = dbset.Where(whereClause);
            }
            foreach (var include in includes)
            {
                dbset = dbset.Include(include);
            }

            return dbset;
        }

        /// <summary>
        /// Gets all of T from the data store by pagging.
        /// </summary>
        /// <param name="paging">paging criteria to determine which page of T to return.</param>
        /// <param name="includes">a params expression array of property names to return in each item of the entity object graph.</param>
        /// <returns>RepositoryResultList of T.</returns>
        public override PagedResultList<TEntity> GetAll(PagingCriteria paging,
            params Expression<Func<TEntity, object>>[] includes)
        {
            PagedResultList<TEntity> result = new PagedResultList<TEntity>();

            var data = GetAll();
            int totalRecords = data.Count();

            foreach (var include in includes)
            {
                data = data.Include(include);
            }

            result.Entities = data.AsQueryable()
                .OrderBy(paging.SortBy + " " + paging.SortDirection)
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize);

            result.PagedMetadata = new PagedMetadata(totalRecords, paging.PageSize, paging.PageNumber);
            return result;
        }

        /// <summary>
        /// Finds all of T from the data store with a pagging.
        /// </summary>
        /// <param name="paging">paging criteria to determine which page of T to return.</param>
        /// <param name="whereClause">where clause to filter entities.</param>
        /// <param name="includes">a params expression array of property names to return in each item of the entity object graph.</param>
        /// <returns>RepositoryResultList of T.</returns>
        public override PagedResultList<TEntity> GetAll(PagingCriteria paging, Expression<Func<TEntity, bool>> whereClause, params Expression<Func<TEntity, object>>[] includes)
        {
            PagedResultList<TEntity> result = new PagedResultList<TEntity>();

            var data = GetAll();
            int totalRecords = data.Count(whereClause);

            foreach (var include in includes)
            {
                data = data.Include(include);
            }

            result.Entities = data.AsQueryable()
                .Where(whereClause)
                .OrderBy(paging.SortBy + " " + paging.SortDirection)
                .Skip((paging.PageNumber - 1)*paging.PageSize)
                .Take(paging.PageSize);

            result.PagedMetadata = new PagedMetadata(totalRecords, paging.PageSize, paging.PageNumber);
            return result;
        }

        public override void Add(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);

            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                Table.Add(entity);
            }
        }

        public override void Edit(TEntity entity)
        {
            AttachIfNot(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public override void Delete(TPrimaryKey id)
        {
            var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = Get(id);
                if (entity == null)
                {
                    return;
                }
            }

            Delete(entity);
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        private bool _isDisposed;

        public override void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._isDisposed)
            {
                return;
            }

            if (disposing)
            {
                this.Context.Dispose();
            }

            this._isDisposed = true;
        }
    }
}