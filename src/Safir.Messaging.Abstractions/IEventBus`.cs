using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Messaging
{
    public interface IEventBus<T> : IObservable<T>
        where T : IEvent
    {
        Task PublishAsync(T message, CancellationToken cancellationToken = default);
    }
}
