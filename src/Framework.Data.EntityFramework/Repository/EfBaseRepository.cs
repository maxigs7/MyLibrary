using System.Data;
using System.Data.Common;
using Framework.Data.EntityFramework.EntityFramework;

namespace Framework.Data.EntityFramework.Repository
{
    /// <summary>
    /// The EF-dependent, base repository for data access
    /// </summary>
    public abstract class EfBaseRepository
    {
        protected EfBaseRepository(DbContextBase dbContext)
        {
            Context = dbContext;
        }

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public DbContextBase Context { get; set; }

        public virtual DbConnection Connection
        {
            get
            {
                var connection = Context.Database.Connection;

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return connection;
            }
        }
    }
}
