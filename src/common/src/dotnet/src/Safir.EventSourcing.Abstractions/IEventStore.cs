using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventStore : IEventStore<Guid> { }
    
    [PublicAPI]
    public interface IEventStore<in TId>
    {
        // TODO: Accept generic event?
        Task AddAsync<TAggregateId>(TAggregateId aggregateId, IEvent @event, CancellationToken cancellationToken = default);

        Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default);

        Task<IEvent> GetAsync<TAggregateId>(TId id, CancellationToken cancellationToken = default);

        Task<TEvent> GetAsync<TEvent, TAggregateId>(TId id, CancellationToken cancellationToken = default)
            where TEvent : IEvent;

        IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);

        IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default);
    }
}
