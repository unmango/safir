using System;

namespace Safir.Messaging.Internal
{
    internal sealed class SubscribeHandlerWrapper<T> : ISubscribeHandlerWrapper
        where T : IEvent
    {
        public IDisposable Subscribe(IEventBus bus, IEventHandler handler)
        {
            if (handler is not IEventHandler<T> typed)
            {
                throw new InvalidOperationException("Incorrect handler type");
            }

            return bus.Subscribe(typed);
        }
    }
}
