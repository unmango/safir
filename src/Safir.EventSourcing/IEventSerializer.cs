using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventSerializer
    {
        ValueTask<Event<TAggregateId>> SerializeAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEvent @event,
            CancellationToken cancellationToken = default);

        ValueTask<IEvent> DeserializeAsync<TAggregateId>(
            Event<TAggregateId> @event,
            CancellationToken cancellationToken = default);

        ValueTask<T> DeserializeAsync<TAggregateId, T>(
            Event<TAggregateId> @event,
            CancellationToken cancellationToken = default)
            where T : IEvent;
    }
}
