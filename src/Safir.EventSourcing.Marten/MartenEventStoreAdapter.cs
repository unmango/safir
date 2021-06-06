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

        public Task AddAsync(long aggregateId, IEvent @event, CancellationToken cancellationToken = default)
        {
            using var session = _store.OpenSession();
            session.Events.Append(AsMartenId(aggregateId), @event);
            return session.SaveChangesAsync(cancellationToken);
        }

        public Task AddAsync(long aggregateId, IEnumerable<IEvent> events, CancellationToken cancellationToken = default)
        {
            using var session = _store.OpenSession();
            session.Events.Append(AsMartenId(aggregateId), events);
            return session.SaveChangesAsync(cancellationToken);
        }

        public Task<IEvent> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            using var session = _store.OpenSession();
            // return session.Events.LoadAsync(id, cancellationToken);
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<IEvent> StreamAsync(
            long aggregateId,
            int startPosition = 0,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default)
        {
            if (startPosition != 0) throw new NotSupportedException("Marten does not support specifying a start position");
            
            using var session = _store.OpenSession();
            var stream = session.Events.FetchStreamAsync(
                AsMartenId(aggregateId),
                endPosition,
                token: cancellationToken);
            
            return stream.AsAsyncEnumerable(cancellationToken).Cast<IEvent>();
        }

        public IAsyncEnumerable<IEvent> StreamBackwardsAsync(
            long aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            using var session = _store.OpenSession();
            var stream = session.Events.FetchStreamAsync(
                AsMartenId(aggregateId),
                token: cancellationToken);

            return stream.AsAsyncEnumerable(cancellationToken)
                .Reverse().Take(count ?? int.MaxValue)
                .Cast<IEvent>();
        }

        private static string AsMartenId(long aggregateId) => aggregateId.ToString();
    }
}
