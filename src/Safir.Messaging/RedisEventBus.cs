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
        private readonly ValueTask<IConnectionMultiplexer> _connectionTask;
        private readonly ILogger<RedisEventBus> _logger;
        private IConnectionMultiplexer? _connection;

        public RedisEventBus(IConnectionPool<IConnectionMultiplexer> connectionPool, ILogger<RedisEventBus> logger)
        {
            _connectionTask = connectionPool.GetConnectionAsync();
            _logger = logger;
        }

        // TODO: I don't like this
        private IConnectionMultiplexer Connection
        {
            get {
                if (_connection != null) return _connection;

                try
                {
                    _connection = _connectionTask.GetAwaiter().GetResult();
                }
                catch (RedisException exception)
                {
                    const string message = "Unable to connect to Redis server";
                    _logger.LogError(exception, message);
                    throw new EventBusException(message, exception);
                }

                return _connection;
            }
        }

        public IObservable<T> GetObservable<T>() where T : IEvent
        {
            _logger.LogTrace("Getting connection subscriber");
            var subscriber = Connection.GetSubscriber();
            _logger.LogTrace("Creating observable from subscriber");
            return subscriber.CreateObservable<T>(typeof(T).Name);
        }

        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : IEvent
        {
            _logger.LogTrace("Getting connection subscriber");
            var subscriber = Connection.GetSubscriber();
            _logger.LogTrace("Publishing message");
            var receivers = await subscriber.PublishAsync(typeof(T).Name, message);
            _logger.LogDebug("{Count} clients received the message", receivers);
        }
    }
}
