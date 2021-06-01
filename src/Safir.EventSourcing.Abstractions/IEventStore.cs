using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventStore
    {
        Task AddAsync(Event @event, CancellationToken cancellationToken = default);

        Task AddAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default);

        Task<Event> GetAsync(long id, CancellationToken cancellationToken = default);

        IAsyncEnumerable<Event> StreamAsync(
            long aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);

        IAsyncEnumerable<Event> StreamBackwardsAsync(
            long aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default);
    }
}
