using Microsoft.Extensions.Logging;
using Safir.Common;
using Safir.Messaging;

namespace Safir.EventSourcing;

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

    public async ValueTask<Event<TAggregateId>> SerializeAsync<TAggregateId>(
        TAggregateId aggregateId,
        IEvent @event,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Serializing event as ReadOnlyMemory<T>");
        var data = await _serializer.SerializeAsMemoryAsync(@event, cancellationToken);
        _logger.LogTrace("Getting event type discriminator");
        var type = await _metadataProvider.GetTypeDiscriminatorAsync(@event, @event.Version, cancellationToken);

        _logger.LogTrace("Constructing serialized event");
        return new Event<TAggregateId>(
            aggregateId,
            type,
            data.ToArray(),
            @event.Occurred,
            @event.GetMetadata(),
            @event.Version);
    }

    public async ValueTask<IEvent> DeserializeAsync<TAggregateId>(
        Event<TAggregateId> @event,
        CancellationToken cancellationToken = default)
    {
        _logger.LogTrace("Getting event type from discriminator");
        var type = await _metadataProvider.GetTypeAsync(@event.Type, @event.Version, cancellationToken);

        _logger.LogTrace("Deserializing event using type discriminator");
        var deserialized = await _serializer.DeserializeAsync(type, @event.Data, cancellationToken);

        if (deserialized is null) throw new InvalidOperationException("Unable to deserialize event");

        return (IEvent)deserialized;
    }

    public ValueTask<T> DeserializeAsync<TAggregateId, T>(
        Event<TAggregateId> @event,
        CancellationToken cancellationToken = default)
        where T : IEvent
    {
        _logger.LogTrace("Deserializing using generic type argument");
        return _serializer.DeserializeAsync<T>(@event.Data, cancellationToken);
    }
}
