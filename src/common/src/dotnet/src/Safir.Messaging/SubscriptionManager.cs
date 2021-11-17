using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Safir.Messaging
{
    internal sealed class SubscriptionManager<T> : IHostedService
        where T : IEvent
    {
        private readonly IServiceProvider _services;
        private readonly IEventBus _eventBus;
        private readonly ILogger<SubscriptionManager<T>> _logger;
        private IDisposable? _subscription;

        public SubscriptionManager(
            IServiceProvider services,
            IEventBus eventBus,
            ILogger<SubscriptionManager<T>> logger)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting subscription manager");
            
            _logger.LogDebug("Subscribing handlers of type {Type}", typeof(T).Name);
            var observable = _eventBus.GetObservable<T>().SelectMany(HandleAsync).Retry();
            
            _logger.LogTrace("Adding subscription of type {Type}", typeof(T).Name);
            _subscription = observable.Subscribe();

            _logger.LogTrace("Exiting StartAsync");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping subscription manager");
            _subscription?.Dispose();

            _logger.LogTrace("Exiting StopAsync");
            return Task.CompletedTask;
        }

        private async Task<T> HandleAsync(T message, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Creating scope for event of type {Type}", typeof(T).Name);
            using var scope = _services.CreateScope();
                
            _logger.LogTrace("Resolving handlers from provider");
            var handlers = scope.ServiceProvider
                .GetRequiredService<IEnumerable<IEventHandler>>()
                .OfType<IEventHandler<T>>();
                
            _logger.LogInformation("Propagating event of type {Type} to handlers", typeof(T).Name);
            await Task.WhenAll(handlers.Select(x => x.HandleAsync(message, cancellationToken)));
            _logger.LogTrace("Returning from propagating event to handlers");

            return message;
        }
    }
}
