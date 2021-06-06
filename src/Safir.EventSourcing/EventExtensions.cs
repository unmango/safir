using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class EventExtensions
    {
        public static ValueTask<T> AggregateAsync<T>(
            this IAsyncEnumerable<IEvent> events,
            CancellationToken cancellationToken)
            where T : IAggregate, new()
        {
            return events.AggregateAsync<T, Guid>(cancellationToken);
        }

        public static async ValueTask<TAggregate> AggregateAsync<TAggregate, TId>(
            this IAsyncEnumerable<IEvent> events,
            CancellationToken cancellationToken)
            where TAggregate : IAggregate<TId>, new()
        {
            TAggregate aggregate = new();

            await foreach (var @event in events.WithCancellation(cancellationToken))
                aggregate.Apply(@event);

            return aggregate;
        }

        public static async IAsyncEnumerable<IEvent> DeserializeAsync(
            this IAsyncEnumerable<Event> events,
            IEventSerializer serializer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var @event in events.WithCancellation(cancellationToken))
                yield return await serializer.DeserializeAsync(@event, cancellationToken);
        }

        public static async IAsyncEnumerable<IEvent> DeserializeAsync<T>(
            this IAsyncEnumerable<Event<T>> events,
            IEventSerializer<T> serializer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var @event in events.WithCancellation(cancellationToken))
                yield return await serializer.DeserializeAsync(@event, cancellationToken);
        }

        public static async IAsyncEnumerable<IEvent> DeserializeAsync<TAggregateId, TId>(
            this IAsyncEnumerable<Event<TAggregateId, TId>> events,
            IEventSerializer<TAggregateId, TId> serializer,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var @event in events.WithCancellation(cancellationToken))
                yield return await serializer.DeserializeAsync(@event, cancellationToken);
        }

        public static Metadata GetMetadata<T>(this T @event)
            where T : IEvent
            => new(@event.CorrelationId, @event.CausationId);
    }
}
