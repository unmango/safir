using System;
using System.Collections.Generic;

namespace Safir.Messaging.Internal
{
    internal interface ISubscribeHandlerWrapper
    {
        IEnumerable<IDisposable> Subscribe(IEventBus bus, IEnumerable<IEventHandler> handlers);
    }
}
