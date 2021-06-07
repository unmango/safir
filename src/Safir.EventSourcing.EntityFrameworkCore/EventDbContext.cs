using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class EventDbContext : DbContext, IEventDbContext
    {
        [PublicAPI]
        public DbSet<Event> Events => Set<Event>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEventConfiguration();
        }
    }
}
