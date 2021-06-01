using System.Threading;
using System.Threading.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public interface IEventSerializer
    {
        ValueTask<Event> SerializeAsync<T>(long aggregateId, T @event, CancellationToken cancellationToken = default)
            where T : IEvent;

        ValueTask<IEvent> DeserializeAsync(Event @event, CancellationToken cancellationToken = default);
    }
}
