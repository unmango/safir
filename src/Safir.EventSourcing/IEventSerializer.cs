using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventSerializer : IEventSerializer<Guid, Guid> { }

    [PublicAPI]
    public interface IEventSerializer<T> : IEventSerializer<T, Guid> { }

    [PublicAPI]
    public interface IEventSerializer<TAggregateId, TId>
    {
        // TODO: The combination of this being generic and IEvent having default interface impl causing issues when
        // serializing. The default values won't be included unless `T @event` is cast as `IEvent` beforehand.
        // Either ditch the default interface impl, or accept `IEvent` instead of a generic `T`
        ValueTask<Event<TAggregateId, TId>> SerializeAsync<T>(
            TAggregateId aggregateId,
            T @event,
            CancellationToken cancellationToken = default)
            where T : IEvent;

        ValueTask<IEvent> DeserializeAsync(Event<TAggregateId, TId> @event, CancellationToken cancellationToken = default);
    }
}
