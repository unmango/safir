using System;
using System.Collections.Generic;
using System.Linq;

namespace Safir.Messaging.Internal
{
    internal static class SubscribeHandlerWrapperExtensions
    {
        public static IEnumerable<IDisposable> Subscribe(
            this ISubscribeHandlerWrapper wrapper,
            IEventBus bus,
            IEnumerable<IEventHandler> handlers)
        {
            return handlers.Select(x => wrapper.Subscribe(bus, x));
        }
    }
}
