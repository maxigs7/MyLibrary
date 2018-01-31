using Framework.Common.Entities.Interfaces;
using Framework.Data.EntityFramework.EntityFramework;

namespace Framework.Data.EntityFramework.Repository
{
    public class EfRepository<TEntity> : EfRepository<TEntity, int>
        where TEntity : class, IEntity
    {
        public EfRepository(DbContextBase dbContext) : base(dbContext)
        {
        }
    }
}
