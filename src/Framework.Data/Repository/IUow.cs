using System;
using System.Threading.Tasks;
using Framework.Common.Entities.Interfaces;

namespace Framework.Data.Repository
{
    public interface IUow : IDisposable
    {
        IRepository<TEntity, TPrimaryKey> GetGeneric<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>;
        IRepository<TEntity> GetGeneric<TEntity>() where TEntity : class, IEntity;
        TRepository GetRepo<TRepository>() where TRepository : class;

        //TRepository GetCustomRepository<TRepository>() where TRepository : class, IRepository;

        void Commit();
        Task CommitAsync();
    }
}
