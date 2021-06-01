using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract record Aggregate : IAggregate
    {
        [NonSerialized]
        private readonly Queue<IEvent> _events = new();
        
        public long Id { get; protected init; }
        
        public int Version { get; protected init; }

        public IEnumerable<IEvent> Events => _events;

        public virtual void Apply(IEvent @event) { }

        public IEnumerable<IEvent> DequeueEvents()
        {
            while (_events.Count > 0)
                yield return _events.Dequeue();
        }

        protected void ClearEvents() => _events.Clear();

        protected void Enqueue(IEvent @event) => _events.Enqueue(@event);
    }
}
