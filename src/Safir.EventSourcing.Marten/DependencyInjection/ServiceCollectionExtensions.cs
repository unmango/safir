using System;
using JetBrains.Annotations;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Safir.EventSourcing.DependencyInjection;

namespace Safir.EventSourcing.Marten.DependencyInjection
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMartenEventStore(this IServiceCollection services, Action<StoreOptions> configure)
        {
            services.AddEventSourcing();
            services.AddMarten(configure);

            services.AddScoped<IEventStore, MartenEventStoreAdapter>();
            services.AddScoped<IAggregateStore, MartenAggregateStoreAdapter>();

            return services;
        }

        public static IServiceCollection UseDefaultAggregateStore(this IServiceCollection services)
        {
            return services.AddScoped<IAggregateStore, DefaultAggregateStore>();
        }
    }
}
