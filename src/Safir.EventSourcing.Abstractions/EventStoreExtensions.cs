using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class EventStoreExtensions
    {
        public static IAsyncEnumerable<IEvent> StreamAsync<TAggregateId, TId>(
            this IEventStore<TAggregateId, TId> store,
            TAggregateId aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync(aggregateId, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamAsync<TAggregateId, TId>(
            this IEventStore<TAggregateId, TId> store,
            TAggregateId aggregateId,
            int startPosition,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync(aggregateId, startPosition, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId, TId>(
            this IEventStore<TAggregateId, TId> store,
            TAggregateId aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamBackwardsAsync(aggregateId, null, cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamFromAsync<TAggregateId, TId>(
            this IEventStore<TAggregateId, TId> store,
            TAggregateId aggregateId,
            int startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, startPosition, cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamUntilAsync<TAggregateId, TId>(
            this IEventStore<TAggregateId, TId> store,
            TAggregateId aggregateId,
            int endPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, endPosition: endPosition, cancellationToken: cancellationToken);
        }
    }
}
