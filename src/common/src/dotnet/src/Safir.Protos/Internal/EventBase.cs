using System;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.Protos.Internal
{
    public abstract class EventBase : IEvent
    {
        [PublicAPI]
        public Guid CausationId { get; init; } = Guid.Empty;
        
        [PublicAPI]
        public Guid CorrelationId { get; init; } = Guid.Empty;

        [PublicAPI]
        public DateTime Occurred { get; } = DateTime.Now;
    }
}
