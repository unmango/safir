using JetBrains.Annotations;

namespace Safir.Messaging;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public interface IEventBus<T> : IObservable<T>
    where T : IEvent
{
    Task PublishAsync(T message, CancellationToken cancellationToken = default);
}
