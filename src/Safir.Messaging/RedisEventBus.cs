using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Safir.Common.ConnectionPool;
using StackExchange.Redis;

namespace Safir.Messaging
{
    internal sealed class RedisEventBus : IEventBus
    {
        private readonly ValueTask<IConnectionMultiplexer> _connectionTask;
        private readonly ILogger<RedisEventBus> _logger;
        private IConnectionMultiplexer? _connection;

        public RedisEventBus(IConnectionPool<IConnectionMultiplexer> connectionPool, ILogger<RedisEventBus> logger)
        {
            _connectionTask = connectionPool.GetConnectionAsync();
            _logger = logger;
        }

        private IConnectionMultiplexer Connection => _connection ??= _connectionTask.Result;
        
        public IObservable<T> GetObservable<T>() where T : IEvent
        {
            _logger.LogTrace("Getting connection subscriber");
            var subscriber = Connection.GetSubscriber();
            _logger.LogTrace("Creating observable from subscriber");
            return subscriber.CreateObservable<T>(nameof(T));
        }

        public Task PublishAsync<T>(T message) where T : IEvent
        {
            _logger.LogTrace("Getting connection subscriber");
            var subscriber = Connection.GetSubscriber();
            _logger.LogTrace("Publishing message");
            return subscriber.PublishAsync(nameof(T), message);
        }
    }
}
