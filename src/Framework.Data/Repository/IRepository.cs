using Framework.Common.Entities.Interfaces;

namespace Framework.Data.Repository
{
    public interface IRepository<TEntity> : IRepository<TEntity, int>
        where TEntity : IEntity
    {
        
    }
}