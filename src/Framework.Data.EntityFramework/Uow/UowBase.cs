using System;
using System.Threading.Tasks;
using Framework.Common.Entities.Interfaces;
using Framework.Data.EntityFramework.EntityFramework;
using Framework.Data.Repository;

namespace Framework.Data.EntityFramework.Uow
{
    public class UowBase<TDbContext> : IUow
        where TDbContext : DbContextBase
    {

        public UowBase(IDbContextProvider<TDbContext> dbContextProvider, IRepositoryProvider repositoryProvider)
        {
            DbContextProvider = dbContextProvider;
            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        protected IDbContextProvider<TDbContext> DbContextProvider { get; set; }
        protected IRepositoryProvider RepositoryProvider { get; set; }
        protected TDbContext DbContext { get { return DbContextProvider.GetDbContext(); } }

        public IRepository<TEntity, TPrimaryKey> GetGeneric<TEntity, TPrimaryKey>() 
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return RepositoryProvider.GetRepositoryForEntityType<TEntity, TPrimaryKey>();
        }

        public IRepository<TEntity> GetGeneric<TEntity>() where TEntity : class, IEntity
        {
            return RepositoryProvider.GetRepositoryForEntityType<TEntity>();
        }

        public TRepository GetRepo<TRepository>() where TRepository : class
        {
            return RepositoryProvider.GetRepository<TRepository>();
        }

        /// <summary>
        ///     Save pending changes to the database
        /// </summary>
        public void Commit()
        {
            DbContext.SaveChanges();
        }

        /// <summary>
        ///     Save pending changes to the database async
        /// </summary>
        public Task CommitAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
    }
}
