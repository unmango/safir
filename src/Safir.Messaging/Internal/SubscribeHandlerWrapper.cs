using System;
using System.Collections.Generic;
using System.Linq;

namespace Safir.Messaging.Internal
{
    internal sealed class SubscribeHandlerWrapper<T> : ISubscribeHandlerWrapper
        where T : IEvent
    {
        public IEnumerable<IDisposable> Subscribe(IEventBus bus, IEnumerable<IEventHandler> handlers)
        {
            return handlers.Cast<IEventHandler<T>>().Select(bus.Subscribe);
        }
    }
}
