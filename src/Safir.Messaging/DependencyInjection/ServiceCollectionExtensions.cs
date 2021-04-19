using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Safir.Messaging.Configuration;
using Safir.Redis.Configuration;
using Safir.Redis.DependencyInjection;

namespace Safir.Messaging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSafirMessaging(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddLogging();
            services.AddOptions<MessagingOptions>();

            services.AddRedisClient();
            services.AddTransient<IEventBus, RedisEventBus>();
            services.AddTransient<IConfigureOptions<RedisOptions>, ConfigureRedisOptions>();
            
            return services;
        }

        public static IServiceCollection AddSafirMessaging(
            this IServiceCollection services,
            Action<MessagingOptions> configure)
        {
            services.AddSafirMessaging();
            services.Configure(configure);
            return services;
        }
    }
}
