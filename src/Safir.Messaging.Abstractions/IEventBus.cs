using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Safir.Messaging
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface IEventBus
    {
        IObservable<T> GetObservable<T>() where T : IEvent;
        
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IEvent;
    }
}
