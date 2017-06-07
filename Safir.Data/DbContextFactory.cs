using Mehdime.Entity;
using System;
using System.Data.Entity;

namespace Safir.Data
{
    public class DbContextFactory : IDbContextFactory
    {
        private DatabaseManager _manager;

        public DbContextFactory(DatabaseManager manager) {
            _manager = manager;
        }

        public TDbContext CreateDbContext<TDbContext>() where TDbContext : DbContext {
            return (TDbContext)Activator.CreateInstance(
                typeof(TDbContext),
                _manager.ConnectionString);
        }
    }
}
