using Safir.Messaging;

namespace Safir.EventSourcing
{
    public static class EventExtensions
    {
        public static Metadata GetMetadata<T>(this T @event)
            where T : IEvent
            => new(@event.CorrelationId, @event.CausationId);
    }
}
