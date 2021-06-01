using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class EventStoreExtensions
    {
        public static Task AddAsync(
            this IEventStore store,
            long aggregateId,
            string type,
            ReadOnlyMemory<byte> data,
            DateTime occurred,
            Guid correlationId,
            Guid causationId,
            int version,
            CancellationToken cancellationToken = default)
        {
            return store.AddAsync(
                aggregateId,
                type,
                data,
                occurred,
                new(correlationId, causationId),
                version,
                cancellationToken);
        }
        
        public static Task AddAsync(
            this IEventStore store,
            long aggregateId,
            string type,
            ReadOnlyMemory<byte> data,
            DateTime occurred,
            Metadata metadata,
            int version,
            CancellationToken cancellationToken = default)
        {
            var @event = new Event(
                aggregateId,
                type,
                data,
                occurred,
                metadata,
                version);

            return store.AddAsync(@event, cancellationToken);
        }
        
        public static IAsyncEnumerable<Event> StreamAsync(
            this IEventStore store,
            long aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync(aggregateId, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<Event> StreamAsync(
            this IEventStore store,
            long aggregateId,
            int startPosition,
            CancellationToken cancellationToken)
        {
            return store.StreamAsync(aggregateId, startPosition, cancellationToken: cancellationToken);
        }

        public static IAsyncEnumerable<Event> StreamBackwardsAsync(
            this IEventStore store,
            long aggregateId,
            CancellationToken cancellationToken)
        {
            return store.StreamBackwardsAsync(aggregateId, null, cancellationToken);
        }

        public static IAsyncEnumerable<Event> StreamFromAsync(
            this IEventStore store,
            long aggregateId,
            int startPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, startPosition, cancellationToken);
        }

        public static IAsyncEnumerable<Event> StreamUntilAsync(
            this IEventStore store,
            long aggregateId,
            int endPosition,
            CancellationToken cancellationToken = default)
        {
            return store.StreamAsync(aggregateId, endPosition: endPosition, cancellationToken: cancellationToken);
        }
    }
}
