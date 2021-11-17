using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    // TODO: Replace with in-memory store
    public class NoOpEventStore : IEventStore
    {
        public Task AddAsync<TAggregateId>(TAggregateId aggregateId, IEvent @event, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync<TAggregateId>(
            TAggregateId aggregateId,
            IEnumerable<IEvent> events,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEvent> GetAsync<TAggregateId>(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TEvent> GetAsync<TEvent, TAggregateId>(Guid id, CancellationToken cancellationToken = default)
            where TEvent : IEvent
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
            TAggregateId aggregateId,
            int startPosition = 0,
            int endPosition = Int32.MaxValue,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
            TAggregateId aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
