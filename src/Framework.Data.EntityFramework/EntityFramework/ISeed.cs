namespace Framework.Data.EntityFramework.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface ISeed<TDbContext> where TDbContext : DbContextBase
    {
        void Seed(TDbContext dbContext);
    }
}