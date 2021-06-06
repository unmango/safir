using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class EventStoreExtensions
    {
        public static IAsyncEnumerable<IEvent> StreamAsync(
            this IEventStore store,
            long aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync(aggregateId, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamAsync(
            this IEventStore store,
            long aggregateId,
            int startPosition,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync(aggregateId, startPosition, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamBackwardsAsync(
            this IEventStore store,
            long aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamBackwardsAsync(aggregateId, null, cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamFromAsync(
            this IEventStore store,
            long aggregateId,
            int startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, startPosition, cancellationToken);
        }

        public static IAsyncEnumerable<IEvent> StreamUntilAsync(
            this IEventStore store,
            long aggregateId,
            int endPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, endPosition: endPosition, cancellationToken: cancellationToken);
        }
    }
}
