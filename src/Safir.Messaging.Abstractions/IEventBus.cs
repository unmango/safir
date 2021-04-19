using System;
using System.Threading.Tasks;

namespace Safir.Messaging
{
    public interface IEventBus
    {
        IObservable<T> GetObservable<T>() where T : IEvent;
        
        Task PublishAsync<T>(T message) where T : IEvent;
    }
}
