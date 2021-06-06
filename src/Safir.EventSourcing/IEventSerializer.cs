using System.Threading;
using System.Threading.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public interface IEventSerializer
    {
        // TODO: The combination of this being generic and IEvent having default interface impl causing issues when
        // serializing. The default values won't be included unless `T @event` is cast as `IEvent` beforehand.
        // Either ditch the default interface impl, or accept `IEvent` instead of a generic `T`
        ValueTask<Event> SerializeAsync<T>(long aggregateId, T @event, CancellationToken cancellationToken = default)
            where T : IEvent;

        ValueTask<IEvent> DeserializeAsync(Event @event, CancellationToken cancellationToken = default);
    }
}
