using Marten;
using Safir.Messaging;

namespace Safir.EventSourcing.Marten;

public class MartenEventStoreAdapter : IEventStore
{
    private readonly IDocumentStore _store;

    public MartenEventStoreAdapter(IDocumentStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public Task AddAsync<TAggregateId>(TAggregateId aggregateId, IEvent @event, CancellationToken cancellationToken = default)
    {
        using var session = _store.OpenSession();
        session.Events.Append(aggregateId, @event);
        return session.SaveChangesAsync(cancellationToken);
    }

    public Task AddAsync<TAggregateId>(
        TAggregateId aggregateId,
        IEnumerable<IEvent> events,
        CancellationToken cancellationToken = default)
    {
        using var session = _store.OpenSession();
        session.Events.Append(aggregateId, events.Cast<object>());
        return session.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEvent> GetAsync<TAggregateId>(Guid id, CancellationToken cancellationToken = default)
    {
        await using var session = _store.OpenSession();
        var loaded = await session.Events.LoadAsync(id, cancellationToken);
        return (IEvent)loaded.Data;
    }

    public Task<TEvent> GetAsync<TEvent, TAggregateId>(Guid id, CancellationToken cancellationToken = default)
        where TEvent : IEvent
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<IEvent> StreamAsync<TAggregateId>(
        TAggregateId aggregateId,
        int startPosition = 0,
        int endPosition = int.MaxValue,
        CancellationToken cancellationToken = default)
    {
        if (startPosition != 0) throw new NotSupportedException("Marten does not support specifying a start position");

        using var session = _store.OpenSession();
        var stream = session.Events.FetchStreamAsync(aggregateId, endPosition, token: cancellationToken);
        return stream.AsAsyncEnumerable(cancellationToken).Cast<IEvent>();
    }

    public IAsyncEnumerable<IEvent> StreamBackwardsAsync<TAggregateId>(
        TAggregateId aggregateId,
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
