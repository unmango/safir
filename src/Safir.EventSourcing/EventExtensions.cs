using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public static class EventExtensions
    {
        public static async ValueTask<T> AggregateAsync<T>(
            this IAsyncEnumerable<IEvent> events,
            CancellationToken cancellationToken)
            where T : IAggregate, new()
        {
            T aggregate = new();
            
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
    }
}
