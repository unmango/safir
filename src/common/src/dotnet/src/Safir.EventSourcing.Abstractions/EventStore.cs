using System;
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
        public abstract Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default);

        public virtual Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(events.Select(x => AddAsync(aggregateId, x, cancellationToken)));
        }

        public abstract Task<IEvent> GetAsync<TAggregateId>(Guid id, CancellationToken cancellationToken = default);

        public virtual async Task<TEvent> GetAsync<TEvent, TAggregateId>(
            Guid id,
            CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            return (TEvent)await GetAsync<TAggregateId>(id, cancellationToken);
        }

        public virtual IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            // TODO: Swapping start/end is inconsistent in impl. For example the DBContext
            // impl doesn't allow start to be after end
            return StreamAsync(aggregateId, int.MaxValue, count ?? 0, cancellationToken);
        }

        public abstract IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);
    }
}
