using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class EventDbContext : DbContext, IEventDbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }
        
        protected EventDbContext() { }
        
        [PublicAPI]
        public DbSet<Event> Events => Set<Event>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEventConfiguration();
        }
    }
    
    public class EventDbContext<T> : DbContext, IEventDbContext<T>
    {
        public EventDbContext(DbContextOptions<EventDbContext<T>> options) : base(options) { }
        
        protected EventDbContext() { }
        
        [PublicAPI]
        public DbSet<Event<T>> Events => Set<Event<T>>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEventConfiguration<T>();
        }
    }
}
