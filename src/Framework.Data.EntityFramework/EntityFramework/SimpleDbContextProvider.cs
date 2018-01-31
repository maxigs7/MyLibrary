using Framework.Common.Entities.Interfaces;

namespace Framework.Data.EntityFramework.EntityFramework
{
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContextBase
    {
        public DbContextBase DbContext { get; }

        public SimpleDbContextProvider(DbContextBase dbContext, IDbContextConfiguration configuration, ICurrentUser currentUser)
        {
            DbContext = dbContext;

            // Set current user if is auth
            DbContext.CurrentUser = currentUser;

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = configuration.ProxyCreationEnabled;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = configuration.LazyLoadingEnabled;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = configuration.ValidateOnSaveEnabled;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }

        public TDbContext GetDbContext()
        {
            return DbContext as TDbContext;
        }

    }
    
}