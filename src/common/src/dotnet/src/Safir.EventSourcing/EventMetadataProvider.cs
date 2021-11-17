using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public abstract class EventMetadataProvider : IEventMetadataProvider
    {
        public abstract string GetTypeDiscriminator<T>(T @event, int version) where T : IEvent;

        public ValueTask<string> GetTypeDiscriminatorAsync<T>(
            T @event,
            int version,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            var discriminator = GetTypeDiscriminator(@event, version);
            return new ValueTask<string>(discriminator);
        }

        public abstract Type GetType(string discriminator, int version);

        public ValueTask<Type> GetTypeAsync(
            string discriminator,
            int version,
            CancellationToken cancellationToken = default)
        {
            var type = GetType(discriminator, version);
            return new ValueTask<Type>(type);
        }
    }
}
