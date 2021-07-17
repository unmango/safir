using System;
using JetBrains.Annotations;

namespace Safir.Messaging
{
    [PublicAPI]
    public interface IEvent
    {
        const int DefaultVersion = 1;
        
        Guid CausationId => Guid.Empty;
        
        Guid CorrelationId => Guid.Empty;
        
        DateTime Occurred { get; }

        int Version => DefaultVersion;
    }
}
