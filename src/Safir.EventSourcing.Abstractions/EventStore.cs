using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract class EventStore : IEventStore
    {
        private readonly ILogger _logger;

        private EventStore() : this(NullLogger<EventStore>.Instance)
        {
        }

        protected EventStore(ILogger<EventStore> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        public abstract Task AddAsync(Event @event, CancellationToken cancellationToken = default);

        public virtual Task AddAsync(IEnumerable<Event> events, CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(events.Select(x => AddAsync((Event)x, cancellationToken)));
        }

        public abstract Task<Event> GetAsync(long id, CancellationToken cancellationToken = default);

        public virtual IAsyncEnumerable<Event> StreamBackwardsAsync(
            long aggregateId,
            int? count = null,
            CancellationToken cancellationToken = default)
        {
            return StreamAsync(aggregateId, int.MaxValue, count ?? int.MinValue, cancellationToken);
        }

        public abstract IAsyncEnumerable<Event> StreamAsync(
            long aggregateId,
            int startPosition = int.MinValue,
            int endPosition = int.MaxValue,
            CancellationToken cancellationToken = default);
    }
}
