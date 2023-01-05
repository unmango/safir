using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.Protos.Internal;

[PublicAPI]
public abstract class EventBase : IEvent
{
    public Guid CausationId { get; init; } = Guid.Empty;

    public Guid CorrelationId { get; init; } = Guid.Empty;

    public DateTime Occurred { get; } = DateTime.Now;
}
