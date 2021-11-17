using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IAggregate : IAggregate<Guid> { }
    
    [PublicAPI]
    public interface IAggregate<out T>
    {
        T Id { get; }
        
        int Version { get; }
        
        IEnumerable<IEvent> Events { get; }

        void Apply(IEvent @event);

        IEnumerable<IEvent> DequeueEvents();
    }
}
