using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using Safir.Common.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing.Marten
{
    public class MartenEventStoreAdapter : IEventStore
    {
        private readonly IDocumentStore _store;

        public MartenEventStoreAdapter(IDocumentStore store)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public Task AddAsync(Guid aggregateId, IEvent @event, CancellationToken cancellationToken = default)
        {
            using var session = _store.OpenSession();
            session.Events.Append(aggregateId, @event);
            return session.SaveChangesAsync(cancellationToken);
        }

        public Task AddAsync(Guid aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {
            using var session = _store.OpenSession();
            session.Events.Append(aggregateId, events);
            return session.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEvent> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await using var session = _store.OpenSession();
            var loaded = await session.Events.LoadAsync(id, cancellationToken);
            return (IEvent)loaded.Data;
        }

        public IAsyncEnumerable<IEvent> StreamAsync(
            Guid aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            if (startPosition != 0) throw new NotSupportedException("Marten does not support specifying a start position");
            
            using var session = _store.OpenSession();
            var stream = session.Events.FetchStreamAsync(aggregateId, endPosition, token: cancellationToken);
            return stream.AsAsyncEnumerable(cancellationToken).Cast<IEvent>();
        }

        public IAsyncEnumerable<IEvent> StreamBackwardsAsync(
            Guid aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            using var session = _store.OpenSession();
            var stream = session.Events.FetchStreamAsync(aggregateId, token: cancellationToken);
            return stream.AsAsyncEnumerable(cancellationToken)
                .Reverse().Take(count ?? int.MaxValue)
                .Cast<IEvent>();
        }
    }
}
