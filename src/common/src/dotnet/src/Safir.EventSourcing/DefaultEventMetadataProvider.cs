using System;
using System.Threading;
using System.Threading.Tasks;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    public sealed class DefaultEventMetadataProvider : IEventMetadataProvider
    {
        public string GetTypeDiscriminator<T>(T @event, int version)
            where T : IEvent
        {
            return typeof(T).AssemblyQualifiedName;
        }

        public ValueTask<string> GetTypeDiscriminatorAsync<T>(
            T @event,
            int version,
            CancellationToken cancellationToken = default)
            where T : IEvent
        {
            return new(GetTypeDiscriminator(@event, version));
        }

        public Type GetType(string discriminator, int version)
        {
            if (string.IsNullOrWhiteSpace(discriminator))
                throw new ArgumentException("Invalid discriminator", nameof(discriminator));

            var result = Type.GetType(discriminator);

            return result ?? throw new InvalidOperationException("Unable to resolve type from discriminator");
        }

        public ValueTask<Type> GetTypeAsync(string discriminator, int version, CancellationToken cancellationToken = default)
        {
            return new(GetType(discriminator, version));
        }
    }
}
