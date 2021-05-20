using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Safir.Agent.Protos;
using Safir.Messaging.Configuration;
using Safir.Redis.Configuration;
using Safir.Redis.DependencyInjection;

namespace Safir.Messaging.DependencyInjection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventHandler<T>(this IServiceCollection services)
            where T : class, IEventHandler
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddSafirMessaging();
            
            services.AddHostedService<SubscriptionManager<FileCreated>>();
            services.AddHostedService<SubscriptionManager<FileChanged>>();
            services.AddHostedService<SubscriptionManager<FileDeleted>>();
            services.AddHostedService<SubscriptionManager<FileRenamed>>();
            
            services.TryAddEnumerable(ServiceDescriptor.Transient<IEventHandler, T>());

            return services;
        }
        
        public static IServiceCollection AddSafirMessaging(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddLogging();
            services.AddOptions<MessagingOptions>();

            services.AddRedisClient();
            services.AddTransient<IEventBus, RedisEventBus>();
            services.AddTransient(typeof(IEventBus<>), typeof(DefaultTypedEventBus<>));
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
