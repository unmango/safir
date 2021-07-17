using System;
using System.ComponentModel;

namespace Safir.Messaging.Internal
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ISubscribeHandlerWrapper
    {
        IDisposable Subscribe(IEventBus bus, IEventHandler handler);

        IDisposable SubscribeRetry(IEventBus bus, IEventHandler handler, Action<Exception> onError);

        IDisposable SubscribeSafe(IEventBus bus, IEventHandler handler, Action<Exception> onError);
    }
}
