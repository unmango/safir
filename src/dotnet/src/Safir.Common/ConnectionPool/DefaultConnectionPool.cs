using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Safir.Common.ConnectionPool
{
    internal sealed class DefaultConnectionPool<T> : IConnectionPool<T>, IAsyncDisposable
    {
        private readonly IConnectionManager<T> _connectionManager;
        private readonly IOptions<ConnectionPoolOptions<T>> _configuration;
        private readonly ILogger<DefaultConnectionPool<T>> _logger;
        private readonly ConcurrentBag<T> _connections = new();
        private readonly Func<IEnumerable<T>, T> _selector;
        private volatile int _roundRobin;

        public DefaultConnectionPool(
            IConnectionManager<T> connectionManager,
            IOptions<ConnectionPoolOptions<T>> configuration,
            ILogger<DefaultConnectionPool<T>> logger)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger;
            
            _selector = configuration.Value.Selector ?? DefaultSelector;
        }

        private int PoolSize => _configuration.Value.PoolSize;
        
        public ValueTask<T> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Getting connection");
            if (_connections.Count == PoolSize)
            {
                _logger.LogDebug("Max connections created, using selector");
                return new(_selector(_connections));
            }

            _logger.LogDebug("Creating new connection");
            return new(Task.Run(async () => {
                var connection = await ConnectAsync(cancellationToken);
                _logger.LogTrace("Adding connection to pool");
                _connections.Add(connection);
                return connection;
            }, cancellationToken));
        }

        public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        public ValueTask DisposeAsync()
        {
            _logger.LogTrace("Disposing connection pool");
            if (_connections.IsEmpty) return new();

            _logger.LogDebug("Disposing managed connections");
            return new(Task.WhenAll(_connections.Select(DisposeConnection)));
        }

        private Task<T> ConnectAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Connecting connection");
            return _connectionManager.ConnectAsync(cancellationToken);
        }

        private Task DisposeConnection(T connection)
        {
            _logger.LogTrace("Disposing connection");
            return _connectionManager.DisposeAsync(connection).AsTask();
        }

        private T DefaultSelector(IEnumerable<T> connections)
        {
            _logger.LogDebug("Using default selector");
            _roundRobin = (_roundRobin + 1) % _connections.Count;
            return connections.Skip(_roundRobin).First();
        }
    }
}
