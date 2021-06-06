using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Safir.Common;

namespace Safir.EventSourcing.DependencyInjection
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventSourcing(this IServiceCollection services)
        {
            services.AddLogging();
            
            services.AddTransient<ISerializer, DefaultSerializer>();
            services.AddTransient<IEventSerializer, DefaultEventSerializer>();
            services.AddTransient(typeof(IEventSerializer<>), typeof(DefaultEventSerializer<>));
            services.AddTransient(typeof(IEventSerializer<,>), typeof(DefaultEventSerializer<,>));
            services.AddTransient<IEventMetadataProvider, DefaultEventMetadataProvider>();
            services.AddTransient<IAggregateStore, DefaultAggregateStore>();
            services.AddTransient(typeof(IAggregateStore<>), typeof(DefaultAggregateStore<>));
            services.AddTransient(typeof(IAggregateStore<,>), typeof(DefaultAggregateStore<,>));
            
            return services;
        }
    }
}
