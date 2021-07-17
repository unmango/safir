using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Safir.Messaging.MediatR.DependencyInjection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatRAdapter<T>(this IServiceCollection services)
            where T : IEvent
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<
                INotificationHandler<Notification<T>>,
                EventBusNotificationPublisher<T>
            >());

            return services;
        }
    }
}
