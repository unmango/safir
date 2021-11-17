using System.Collections.Generic;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public static class AggregateExtensions
    {
        public static void Apply<T>(this IAggregate<T> aggregate, IEnumerable<IEvent> events)
        {
            foreach (var @event in events) aggregate.Apply(@event);
        }
    }
}
