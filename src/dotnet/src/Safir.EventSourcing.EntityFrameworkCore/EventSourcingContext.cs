using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class EventSourcingContext : DbContext, IEventDbContext
    {
        public EventSourcingContext(DbContextOptions<EventSourcingContext> options) : base(options) { }
        
        protected EventSourcingContext() { }
        
        [PublicAPI]
        public DbSet<Event> Events => Set<Event>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEventConfiguration();
        }
    }
    
    public class EventSourcingContext<T> : DbContext, IEventDbContext<T>
    {
        public EventSourcingContext(DbContextOptions<EventSourcingContext<T>> options) : base(options) { }
        
        protected EventSourcingContext() { }
        
        [PublicAPI]
        public DbSet<Event<T>> Events => Set<Event<T>>();
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEventConfiguration<T>();
        }
    }
}
