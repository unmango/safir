using System;
using Microsoft.Extensions.DependencyInjection;
using Safir.Common.ConnectionPool.DependencyInjection;
using Safir.Redis.Configuration;
using StackExchange.Redis;

namespace Safir.Redis.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisClient(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddLogging();
            services.AddOptions<RedisOptions>();
            
            services.AddConnectionPool<IConnectionMultiplexer, CreateRedisConnection>();
            services.AddTransient<IRedisClient, DefaultRedisClient>();
            
            return services;
        }

        public static IServiceCollection AddRedisClient(
            this IServiceCollection services,
            Action<RedisOptions> configure)
        {
            services.AddRedisClient();
            services.Configure(configure);
            return services;
        }
    }
}
