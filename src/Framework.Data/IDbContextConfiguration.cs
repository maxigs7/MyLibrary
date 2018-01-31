namespace Framework.Data
{
    public interface IDbContextConfiguration
    {
        bool ProxyCreationEnabled { get; set; }
        bool LazyLoadingEnabled { get; set; }
        bool ValidateOnSaveEnabled { get; set; }
        bool AutoDetectChangesEnabled { get; set; }
    }
}