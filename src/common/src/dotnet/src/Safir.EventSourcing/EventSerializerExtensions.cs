using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class EventSerializerExtensions
    {
        public static async ValueTask<Event> SerializeAsync(
            this IEventSerializer serializer,
            Guid aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default)
        {
            return (Event)await serializer.SerializeAsync(aggregateId, @event, cancellationToken);
        }

        // TODO: Ensure this doesn't cause ambiguity errors
        public static ValueTask<Event<TAggregateId>> SerializeAsync<TAggregateId, T>(
            this IEventSerializer serializer,
            TAggregateId aggregateId,
            T @event,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return serializer.SerializeAsync(aggregateId, @event, cancellationToken);
        }

        public static ValueTask<IEvent> DeserializeAsync(
            this IEventSerializer serializer,
            Event @event,
            CancellationToken cancellationToken = default)
        {
            return serializer.DeserializeAsync(@event, cancellationToken);
        }

        public static ValueTask<T> DeserializeAsync<T>(
            this IEventSerializer serializer,
            Event @event,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return serializer.DeserializeAsync<Guid, T>(@event, cancellationToken);
        }
    }
}
