using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract record Aggregate : Aggregate<Guid>, IAggregate
    {
        protected Aggregate() => Id = Guid.NewGuid();
    }
    
    [PublicAPI]
    public abstract record Aggregate<T> : IAggregate<T>
    {
        [NonSerialized]
        private readonly Queue<IEvent> _events = new();

        public T Id { get; protected init; } = default!; // TODO: Nullability
        
        public int Version { get; protected set; }

        public IEnumerable<IEvent> Events => _events;

        protected virtual void Apply(IEvent @event) => TryUpdateVersion(@event);

        void IAggregate<T>.Apply(IEvent @event) => Apply(@event);

        public IEnumerable<IEvent> DequeueEvents()
        {
            while (_events.Count > 0)
                yield return _events.Dequeue();
        }

        protected void ClearEvents() => _events.Clear();

        protected void Enqueue(IEvent @event) => _events.Enqueue(@event);

        protected bool TryUpdateVersion(IEvent @event)
        {
            return (Version = Math.Max(Version, @event.Version)) == @event.Version;
        }
    }
}
