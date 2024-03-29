using JetBrains.Annotations;

namespace Safir.Messaging;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public interface IEventBus
{
    Task<IDisposable> SubscribeAsync<T>(IObserver<T> observer, CancellationToken cancellationToken = default)
        where T : IEvent;

    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : IEvent;
}
