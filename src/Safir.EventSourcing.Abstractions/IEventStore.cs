using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventStore
    {
        // TODO: Accept generic event?
        Task AddAsync(long aggregateId, IEvent @event, CancellationToken cancellationToken = default);

        Task AddAsync(long aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken = default);

        Task<IEvent> GetAsync(long id, CancellationToken cancellationToken = default);

        IAsyncEnumerable<IEvent> StreamAsync(
            long aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);

        IAsyncEnumerable<IEvent> StreamBackwardsAsync(
            long aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default);
    }
}
