using System;
using System.Collections.Generic;
using System.Data.Entity;
using Framework.Common.Entities.Interfaces;
using Framework.Data.EntityFramework.Repository;
using Framework.Data.Repository;

namespace Framework.Data.EntityFramework.EntityFramework
{
    /// <summary>
    /// A maker of Code Camper Repositories.
    /// </summary>
    /// <remarks>
    /// An instance of this class contains repository factory functions for different types.
    /// Each factory function takes an EF <see cref="DbContextBase"/> and returns
    /// a repository bound to that DbContext.
    /// <para>
    /// Designed to be a "Singleton", configured at web application start with
    /// all of the factory functions needed to create any type of repository.
    /// Should be thread-safe to use because it is configured at app start,
    /// before any request for a factory, and should be immutable thereafter.
    /// </para>
    /// </remarks>
    public class RepositoryFactories
    {
        /// <summary>
        /// Return the runtime Code Camper repository factory functions,
        /// each one is a factory for a repository of a particular type.
        /// </summary>
        /// <remarks>
        /// OVERRIDE THIS METHOD TO ADD CUSTOM FACTORY FUNCTIONS
        /// </remarks>
        protected virtual IDictionary<Type, Func<DbContextBase, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContextBase, object>>
            {
                //{typeof(ICustomRepoRepository), dbContext => new ReporteRepository(dbContext)}
            };
        }

        /// <summary>
        /// Constructor that initializes with runtime Code Camper repository factories
        /// </summary>
        public RepositoryFactories()
        {
            _repositoryFactories = GetFactories();
        }

        /// <summary>
        /// Constructor that initializes with an arbitrary collection of factories
        /// </summary>
        /// <param name="factories">
        /// The repository factory functions for this instance. 
        /// </param>
        /// <remarks>
        /// This ctor is primarily useful for testing this class
        /// </remarks>
        public RepositoryFactories(IDictionary<Type, Func<DbContextBase, object>> factories)
        {
            _repositoryFactories = factories;
        }

        /// <summary>
        /// Get the repository factory function for the type.
        /// </summary>
        /// <typeparam name="T">Type serving as the repository factory lookup key.</typeparam>
        /// <returns>The repository function if found, else null.</returns>
        /// <remarks>
        /// The type parameter, T, is typically the repository type 
        /// but could be any type (e.g., an entity type)
        /// </remarks>
        public Func<DbContextBase, object> GetRepositoryFactory<T>()
        {
            Func<DbContextBase, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        /// <summary>
        /// Get the factory for <see cref="IRepository{T, TKey}"/> where T is an entity type.
        /// </summary>
        /// <typeparam name="TEntity">The root type of the repository, typically an entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The root type of the repository, typically an entity key.</typeparam>
        /// <returns>
        /// A factory that creates the <see cref="IRepository{T, TKey}"/>, given an EF <see cref="DbContext"/>.
        /// </returns>
        /// <remarks>
        /// Looks first for a custom factory in <see cref="_repositoryFactories"/>.
        /// If not, falls back to the <see cref="DefaultEntityRepositoryFactory{T}"/>.
        /// You can substitute an alternative factory for the default one by adding
        /// a repository factory for type "T" to <see cref="_repositoryFactories"/>.
        /// </remarks>
        public Func<DbContextBase, object> GetRepositoryFactoryForEntityType<TEntity, TPrimaryKey>() 
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return GetRepositoryFactory<IRepository<TEntity, TPrimaryKey>>() ?? DefaultEntityRepositoryFactory<TEntity, TPrimaryKey>();
        }

        /// <summary>
        /// Get the factory for <see cref="IRepository{T, TKey}"/> where T is an entity type.
        /// </summary>
        /// <typeparam name="TEntity">The root type of the repository, typically an entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The root type of the repository, typically an entity key.</typeparam>
        /// <returns>
        /// A factory that creates the <see cref="IRepository{T, TKey}"/>, given an EF <see cref="DbContext"/>.
        /// </returns>
        /// <remarks>
        /// Looks first for a custom factory in <see cref="_repositoryFactories"/>.
        /// If not, falls back to the <see cref="DefaultEntityRepositoryFactory{T}"/>.
        /// You can substitute an alternative factory for the default one by adding
        /// a repository factory for type "T" to <see cref="_repositoryFactories"/>.
        /// </remarks>
        public Func<DbContextBase, object> GetRepositoryFactoryForEntityType<TEntity>()
            where TEntity : class, IEntity
        {
            return GetRepositoryFactory<IRepository<TEntity>>() ?? DefaultEntityRepositoryFactory<TEntity>();
        }

        /// <summary>
        /// Default factory for a <see cref="IRepository{T,TKey}"/> where T is an entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of the repository's root entity</typeparam>
        /// <typeparam name="TPrimaryKey">Type of the repository's root entity key</typeparam>
        protected virtual Func<DbContextBase, object> DefaultEntityRepositoryFactory<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return dbContext => new EfRepository<TEntity, TPrimaryKey>(dbContext);
        }

        /// <summary>
        /// Default factory for a <see cref="IRepository{T,TKey}"/> where T is an entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of the repository's root entity</typeparam>
        protected virtual Func<DbContextBase, object> DefaultEntityRepositoryFactory<TEntity>()
            where TEntity : class, IEntity
        {
            return dbContext => new EfRepository<TEntity>(dbContext);
        }

        /// <summary>
        /// Get the dictionary of repository factory functions.
        /// </summary>
        /// <remarks>
        /// A dictionary key is a System.Type, typically a repository type.
        /// A value is a repository factory function
        /// that takes a <see cref="DbContextBase"/> argument and returns
        /// a repository object. Caller must know how to cast it.
        /// </remarks>
        private readonly IDictionary<Type, Func<DbContextBase, object>> _repositoryFactories;
    }
}