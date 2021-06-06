using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Marten;

namespace Safir.EventSourcing.Marten
{
    public class MartenAggregateStoreAdapter : IAggregateStore
    {
        private readonly IDocumentStore _store;
        private readonly IEventStore _eventStore;

        public MartenAggregateStoreAdapter(IDocumentStore store, IEventStore eventStore)
        {
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        public Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default) where T : IAggregate
        {
            var events = aggregate.DequeueEvents().ToList();

            if (!events.Any()) return Task.CompletedTask;

            return _eventStore.AddAsync(aggregate.Id, events, cancellationToken);
        }

        public ValueTask<T> GetAsync<T>(long id, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            // TODO: State needs to be class, what do?
            // using var session = _store.OpenSession();
            // var task = session.Events.AggregateStreamAsync(
            //     id.ToString(),
            //     state: new T(),
            //     token: cancellationToken);
            //
            // return new ValueTask<T>(task);

            throw new NotImplementedException();
        }

        public ValueTask<T> GetAsync<T>(long id, int version, CancellationToken cancellationToken = default)
            where T : IAggregate, new()
        {
            throw new System.NotImplementedException();
        }
    }
}
