using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Common.ConnectionPool;
using Safir.Redis.Configuration;
using StackExchange.Redis;

namespace Safir.Redis
{
    [UsedImplicitly]
    internal sealed class CreateRedisConnection : ICreateConnection<IConnectionMultiplexer>
    {
        private readonly IOptions<RedisOptions> _options;
        private readonly ILogger<CreateRedisConnection> _logger;

        public CreateRedisConnection(IOptions<RedisOptions> options, ILogger<CreateRedisConnection> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }
        
        public async Task<IConnectionMultiplexer> ConnectAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Parsing redis configuration");
            var config = ConfigurationOptions.Parse(_options.Value.Configuration);
            _logger.LogDebug("Connecting to redis instance");
            return await ConnectionMultiplexer.ConnectAsync(config);
        }
    }
}
