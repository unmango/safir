using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract class EventStore : IEventStore
    {
        public abstract Task AddAsync(long aggregateId, IEvent @event, CancellationToken cancellationToken = default);

        public virtual Task AddAsync(
            long aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(events.Select(x => AddAsync(aggregateId, x, cancellationToken)));
        }

        public abstract Task<IEvent> GetAsync(long id, CancellationToken cancellationToken = default);

        public virtual IAsyncEnumerable<IEvent> StreamBackwardsAsync(
            long aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            return StreamAsync(aggregateId, int.MaxValue, count ?? int.MinValue, cancellationToken);
        }

        public abstract IAsyncEnumerable<IEvent> StreamAsync(
            long aggregateId,
            int startPosition = int.MinValue,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);
    }
}
