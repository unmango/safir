using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;
using Safir.Messaging.MediatR;

// ReSharper disable once CheckNamespace
namespace MediatR
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class PublisherExtensions
    {
        public static Task Publish<T>(
            this IPublisher publisher,
            T @event,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return publisher.Publish(@event.AsNotification(), cancellationToken);
        }
    }
}
