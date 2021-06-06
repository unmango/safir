using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Safir.Common;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public class DefaultEventSerializer : IEventSerializer
    {
        private readonly ISerializer _serializer;
        private readonly IEventMetadataProvider _metadataProvider;
        private readonly ILogger<DefaultEventSerializer> _logger;

        public DefaultEventSerializer(
            ISerializer serializer,
            IEventMetadataProvider metadataProvider,
            ILogger<DefaultEventSerializer> logger)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async ValueTask<Event> SerializeAsync<T>(
            long aggregateId,
            T @event,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            _logger.LogTrace("Serializing event as ReadOnlyMemory<T>");
            var data = await _serializer.SerializeAsMemoryAsync(@event, cancellationToken);
            _logger.LogTrace("Getting event type discriminator");
            var type = await _metadataProvider.GetTypeDiscriminatorAsync(@event, @event.Version, cancellationToken);

            _logger.LogTrace("Constructing serialized event");
            return new Event(aggregateId, type, data.ToArray(), @event.Occurred, @event.GetMetadata(), @event.Version);
        }

        public async ValueTask<IEvent> DeserializeAsync(Event @event, CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Getting event type from discriminator");
            var type = await _metadataProvider.GetTypeAsync(@event.Type, @event.Version, cancellationToken);

            _logger.LogTrace("Deserializing event using type discriminator");
            return (IEvent)await _serializer.DeserializeAsync(type, @event.Data, cancellationToken);
        }
    }
}
