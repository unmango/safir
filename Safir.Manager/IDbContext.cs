using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Safir.Manager
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entityToDelete) where TEntity : class;
        int SaveChanges();
        void Dispose();
    }
}
