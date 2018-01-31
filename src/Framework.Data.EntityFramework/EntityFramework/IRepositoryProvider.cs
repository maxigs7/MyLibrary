using System;
using Framework.Common.Entities.Interfaces;
using Framework.Data.Repository;

namespace Framework.Data.EntityFramework.EntityFramework
{
    /// <summary>
    /// Interface for a class that can provide repositories by type.
    /// The class may create the repositories dynamically if it is unable
    /// to find one in its cache of repositories.
    /// </summary>
    public interface IRepositoryProvider
    {
        /// <summary>
        /// Get and set the <see cref="DbContext"/> with which to initialize a repository
        /// if one must be created.
        /// </summary>
        DbContextBase DbContext { get; set; }

        /// <summary>
        /// Get an <see cref="IRepository{TEntity, TPrimaryKey}"/> for entity type, T.
        /// </summary>
        /// <typeparam name="TEntity">
        /// Root entity type of the <see cref="IRepository{TEntity, TPrimaryKey}"/>.
        /// </typeparam>
        /// <typeparam name="TPrimaryKey">
        /// </typeparam>
        IRepository<TEntity, TPrimaryKey> GetRepositoryForEntityType<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>;

        /// <summary>
        /// Get an <see cref="IRepository"/> for entity type, T.
        /// </summary>
        /// <typeparam name="TEntity">
        /// Root entity type of the <see cref="IRepository"/>.
        /// </typeparam>
        IRepository<TEntity> GetRepositoryForEntityType<TEntity>() where TEntity : class, IEntity;

        /// <summary>
        /// Get a repository of type T.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the repository, typically a custom repository interface.
        /// </typeparam>
        /// <param name="factory">
        /// An optional repository creation function that takes a <see cref="DbContext"/>
        /// and returns a repository of T. Used if the repository must be created.
        /// </param>
        /// <remarks>
        /// Looks for the requested repository in its cache, returning if found.
        /// If not found, tries to make one with the factory, fallingback to 
        /// a default factory if the factory parameter is null.
        /// </remarks>
        T GetRepository<T>(Func<DbContextBase, object> factory = null) where T : class;

        /// <summary>
        /// Set the repository to return from this provider.
        /// </summary>
        /// <remarks>
        /// Set a repository if you don't want this provider to create one.
        /// Useful in testing and when developing without a backend
        /// implementation of the object returned by a repository of type T.
        /// </remarks>
        void SetRepository<T>(T repository);
    }
}