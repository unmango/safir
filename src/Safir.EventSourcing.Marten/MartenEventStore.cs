using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MartenIEventStore = Marten.Events.IEventStore;

namespace Safir.EventSourcing.Marten
{
    public class MartenEventStore : IEventStore
    {
        private readonly MartenIEventStore _martenStore;

        public MartenEventStore(MartenIEventStore martenStore)
        {
            _martenStore = martenStore ?? throw new ArgumentNullException(nameof(martenStore));
        }

        public Task AddAsync(Event @event, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Event> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Event> StreamAsync(
            long aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Event> StreamBackwardsAsync(
            long aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
