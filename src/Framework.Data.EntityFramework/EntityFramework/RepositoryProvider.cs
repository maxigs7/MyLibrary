using System;
using System.Collections.Generic;
using Framework.Common.Entities.Interfaces;
using Framework.Data.Repository;

namespace Framework.Data.EntityFramework.EntityFramework
{
    /// <summary>
    /// Provides an <see cref="IRepository{T}"/> for a client request.
    /// </summary>
    /// <remarks>
    /// Caches repositories of a given type so that repositories are only created once per provider.
    /// Code Camper creates a new provider per client request.
    /// </remarks>
    public class RepositoryProvider : IRepositoryProvider
    {
        public RepositoryProvider(RepositoryFactories repositoryFactories)
        {
            _repositoryFactories = repositoryFactories;
            Repositories = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Get and set the <see cref="DbContext"/> with which to initialize a repository
        /// if one must be created.
        /// </summary>
        public DbContextBase DbContext { get; set; }

        /// <summary>
        /// Get or create-and-cache the default <see cref="IRepository{T, TKey}"/> for an entity of type T.
        /// </summary>
        /// <typeparam name="TEntity">
        /// Root entity type of the <see cref="IRepository{T,TKey}"/>.
        /// </typeparam>
        /// <typeparam name="TPrimaryKey">
        /// Root entity key type of the <see cref="IRepository{T,TKey}"/>.
        /// </typeparam>
        /// <remarks>
        /// If can't find repository in cache, use a factory to create one.
        /// </remarks>
        public IRepository<TEntity, TPrimaryKey> GetRepositoryForEntityType<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return GetRepository<IRepository<TEntity, TPrimaryKey>>(
                _repositoryFactories.GetRepositoryFactoryForEntityType<TEntity, TPrimaryKey>());
        }

        /// <summary>
        /// Get or create-and-cache the default <see cref="IRepository"/> for an entity of type T.
        /// </summary>
        /// <typeparam name="TEntity">
        /// Root entity type of the <see cref="IRepository{T,TKey}"/>.
        /// </typeparam>
        /// <remarks>
        /// If can't find repository in cache, use a factory to create one.
        /// </remarks>
        public IRepository<TEntity> GetRepositoryForEntityType<TEntity>() where TEntity : class, IEntity
        {
            return GetRepository<IRepository<TEntity>>(
                _repositoryFactories.GetRepositoryFactoryForEntityType<TEntity>());
        }


        /// <summary>
        /// Get or create-and-cache a repository of type T.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the repository, typically a custom repository interface.
        /// </typeparam>
        /// <param name="factory">
        /// An optional repository creation function that takes a DbContext argument
        /// and returns a repository of T. Used if the repository must be created and
        /// caller wants to specify the specific factory to use rather than one
        /// of the injected <see cref="RepositoryFactories"/>.
        /// </param>
        /// <remarks>
        /// Looks for the requested repository in its cache, returning if found.
        /// If not found, tries to make one using <see cref="MakeRepository{T}"/>.
        /// </remarks>
        public virtual T GetRepository<T>(Func<DbContextBase, object> factory = null) where T : class
        {
            // Look for T dictionary cache under typeof(T).
            object repoObj;
            Repositories.TryGetValue(typeof(T), out repoObj);
            if (repoObj != null)
            {
                return (T) repoObj;
            }

            // Not found or null; make one, add to dictionary cache, and return it.
            return MakeRepository<T>(factory, DbContext);
        }

        /// <summary>
        /// Get the dictionary of repository objects, keyed by repository type.
        /// </summary>
        /// <remarks>
        /// Caller must know how to cast the repository object to a useful type.
        /// <p>This is an extension point. You can register fully made repositories here
        /// and they will be used instead of the ones this provider would otherwise create.</p>
        /// </remarks>
        protected Dictionary<Type, object> Repositories { get; }

        /// <summary>Make a repository of type T.</summary>
        /// <typeparam name="T">Type of repository to make.</typeparam>
        /// <param name="dbContext">
        /// The <see cref="DbContext"/> with which to initialize the repository.
        /// </param>        
        /// <param name="factory">
        /// Factory with <see cref="DbContext"/> argument. Used to make the repository.
        /// If null, gets factory from <see cref="_repositoryFactories"/>.
        /// </param>
        /// <returns></returns>
        protected virtual T MakeRepository<T>(Func<DbContextBase, object> factory, DbContextBase dbContext)
        {
            var f = factory ?? _repositoryFactories.GetRepositoryFactory<T>();
            if (f == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
            }
            var repo = (T) f(dbContext);
            Repositories[typeof(T)] = repo;
            return repo;
        }

        /// <summary>
        /// Set the repository for type T that this provider should return.
        /// </summary>
        /// <remarks>
        /// Plug in a custom repository if you don't want this provider to create one.
        /// Useful in testing and when developing without a backend
        /// implementation of the object returned by a repository of type T.
        /// </remarks>
        public void SetRepository<T>(T repository)
        {
            Repositories[typeof(T)] = repository;
        }

        /// <summary>
        /// The <see cref="RepositoryFactories"/> with which to create a new repository.
        /// </summary>
        /// <remarks>
        /// Should be initialized by constructor injection
        /// </remarks>
        private readonly RepositoryFactories _repositoryFactories;
    }
}