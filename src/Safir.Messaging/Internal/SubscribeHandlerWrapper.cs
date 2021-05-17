using System;

namespace Safir.Messaging.Internal
{
    internal sealed class SubscribeHandlerWrapper<T> : ISubscribeHandlerWrapper
        where T : IEvent
    {
        public IDisposable Subscribe(IEventBus bus, IEventHandler handler)
        {
            return SubscribeSafe(bus, handler, _ => { });
        }

        public IDisposable SubscribeRetry(IEventBus bus, IEventHandler handler, Action<Exception> onError)
        {
            return bus.SubscribeRetry(ValidateHandler(handler), onError);
        }

        public IDisposable SubscribeSafe(IEventBus bus, IEventHandler handler, Action<Exception> onError)
        {
            return bus.SubscribeSafe(ValidateHandler(handler), onError);
        }

        private static IEventHandler<T> ValidateHandler(IEventHandler handler)
        {
            if (handler is not IEventHandler<T> typed)
            {
                throw new InvalidOperationException("Incorrect handler type");
            }

            return typed;
        }
    }
}
