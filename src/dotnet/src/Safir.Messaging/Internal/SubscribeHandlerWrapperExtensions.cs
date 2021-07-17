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
        
        public static IEnumerable<IDisposable> SubscribeRetry(
            this ISubscribeHandlerWrapper wrapper,
            IEventBus bus,
            IEnumerable<IEventHandler> handlers,
            Action<Exception> onError)
        {
            return handlers.Select(x => wrapper.SubscribeRetry(bus, x, onError));
        }
        
        public static IEnumerable<IDisposable> SubscribeSafe(
            this ISubscribeHandlerWrapper wrapper,
            IEventBus bus,
            IEnumerable<IEventHandler> handlers,
            Action<Exception> onError)
        {
            return handlers.Select(x => wrapper.SubscribeSafe(bus, x, onError));
        }
    }
}
