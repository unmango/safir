using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Safir.EventSourcing.EntityFrameworkCore
{
    [PublicAPI]
    public interface IEventDbContext
    {
        DbSet<Event> Events { get; }
    }
    
    [PublicAPI]
    public interface IEventDbContext<T>
    {
        DbSet<Event<T>> Events { get; }
    }
}
