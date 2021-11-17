using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Safir.Messaging
{
    internal sealed class DefaultTypedEventBus<T> : IEventBus<T>
        where T : IEvent
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<DefaultTypedEventBus<T>> _logger;

        public DefaultTypedEventBus(IEventBus eventBus, ILogger<DefaultTypedEventBus<T>> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(DefaultTypedEventBus<T>));
            _logger = logger;
        }
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            _logger.LogDebug("Delegating subscription to generic event bus");
            return _eventBus.GetObservable<T>().Subscribe(observer);
        }

        public Task PublishAsync(T message, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Delegating publish to generic event bus");
            return _eventBus.PublishAsync(message, cancellationToken);
        }
    }
}
