using Safir.Common;
using Safir.Common.Internal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IEventDispatcher, DefaultEventDispatcher>();

            return services;
        }
    }
}
