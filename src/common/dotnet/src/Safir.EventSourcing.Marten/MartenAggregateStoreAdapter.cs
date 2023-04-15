using Marten;
using Microsoft.Extensions.Logging;

namespace Safir.EventSourcing.Marten;

public class MartenAggregateStoreAdapter : IAggregateStore
{
    private readonly IDocumentStore _store;
    private readonly IEventStore _eventStore;
    private readonly ILogger<MartenAggregateStoreAdapter> _logger;

    public MartenAggregateStoreAdapter(
        IDocumentStore store,
        IEventStore eventStore,
        ILogger<MartenAggregateStoreAdapter> logger)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task StoreAsync<T>(T aggregate, CancellationToken cancellationToken = default)
        where T : class, IAggregate
    {
        _logger.LogTrace("Dequeuing events from aggregate");
        var events = aggregate.DequeueEvents().ToList();

        if (!events.Any()) return Task.CompletedTask;

        _logger.LogTrace("Adding events to event store");
        return _eventStore.AddAsync(aggregate.Id, events, cancellationToken);
    }

    public Task StoreAsync<TAggregate, TId>(TAggregate aggregate, CancellationToken cancellationToken = default)
        where TAggregate : IAggregate<TId>
    {
        throw new NotImplementedException();
    }

    public ValueTask<T> GetAsync<T>(Guid id, CancellationToken cancellationToken = default)
        where T : class, IAggregate, new()
    {
        using var session = _store.OpenSession();
        var task = session.Events.AggregateStreamAsync(
            id.ToString(),
            state: new T(),
            token: cancellationToken);

        return new ValueTask<T>(task!); // TODO: Nullability
    }

    public ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, CancellationToken cancellationToken = default)
        where TAggregate : IAggregate<TId>, new()
    {
        throw new NotImplementedException();
    }

    public ValueTask<T> GetAsync<T>(Guid id, int version, CancellationToken cancellationToken = default)
        where T : class, IAggregate, new()
    {
        using var session = _store.OpenSession();
        var task = session.Events.AggregateStreamAsync(
            id.ToString(),
            version,
            state: new T(),
            token: cancellationToken);

        return new ValueTask<T>(task!); // TODO: Nullability
    }

    public ValueTask<TAggregate> GetAsync<TAggregate, TId>(TId id, int version, CancellationToken cancellationToken = default)
        where TAggregate : IAggregate<TId>, new()
    {
        throw new NotImplementedException();
    }
}
