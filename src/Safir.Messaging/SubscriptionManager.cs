using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Safir.Messaging
{
    internal sealed class SubscriptionManager : IHostedService
    {
        private readonly IEnumerable<IEventHandler> _handlers;
        private readonly IEventBus _eventBus;
        private readonly ILogger<SubscriptionManager> _logger;
        private readonly List<IDisposable> _subscriptions = new();

        public SubscriptionManager(
            IEnumerable<IEventHandler> handlers,
            IEventBus eventBus,
            ILogger<SubscriptionManager> logger)
        {
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting subscription manager");
            foreach (var group in _handlers.GroupByEvent())
            {
                _logger.LogTrace("Subscribing handlers for event type {Type}", group.Key);
                foreach (var handler in group)
                {
                    try
                    {
                        _subscriptions.Add(_eventBus.Subscribe(group.Key, handler));
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error while subscribing handler");
                    }
                }
            }
            
            _logger.LogTrace("Exiting StartAsync");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping subscription manager");
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }
            
            _logger.LogTrace("Exiting StopAsync");
            return Task.CompletedTask;
        }
    }
}
