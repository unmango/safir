using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    public class EventDbContext : DbContext
    {
        [UsedImplicitly]
        public DbSet<Event> Events { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureEvents();
        }
    }
}
