namespace Framework.Data.EntityFramework.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<TDbContext>
        where TDbContext : DbContextBase
    {
        TDbContext GetDbContext();

    }
}