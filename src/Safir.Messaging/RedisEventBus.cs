using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Safir.Common.ConnectionPool;
using StackExchange.Redis;

namespace Safir.Messaging
{
    internal sealed class RedisEventBus : IEventBus
    {
        private readonly IConnectionPool<IConnectionMultiplexer> _connectionPool;
        private readonly ILogger<RedisEventBus> _logger;

        public RedisEventBus(IConnectionPool<IConnectionMultiplexer> connectionPool, ILogger<RedisEventBus> logger)
        {
            _connectionPool = connectionPool ?? throw new ArgumentNullException(nameof(connectionPool));
            _logger = logger;
        }

        public async Task<IDisposable> SubscribeAsync<T>(Action<T> callback, CancellationToken cancellationToken = default)
            where T : IEvent
        {
            var connection = await GetConnectionAsync(cancellationToken);
            _logger.LogTrace("Getting connection subscriber");
            var subscriber = connection.GetSubscriber();
            _logger.LogTrace("Creating observable from subscriber");
            return await subscriber.SubscribeAsync(typeof(T).Name, callback);
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IEvent
        {
            var connection = await GetConnectionAsync(cancellationToken);
            _logger.LogTrace("Getting connection subscriber");
            var subscriber = connection.GetSubscriber();
            _logger.LogTrace("Publishing message");
            var receivers = await subscriber.PublishAsync(typeof(T).Name, message);
            _logger.LogDebug("{Count} clients received the message", receivers);
        }

        private async Task<IConnectionMultiplexer> GetConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogTrace("Getting connection from pool");
                return await _connectionPool.GetConnectionAsync(cancellationToken);
            }
            catch (RedisException exception)
            {
                const string message = "Unable to connect to Redis server";
                _logger.LogError(exception, message);
                throw new EventBusException(message, exception);
            }
        }
    }
}
