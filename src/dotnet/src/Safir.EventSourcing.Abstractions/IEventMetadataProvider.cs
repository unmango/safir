using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Safir.Messaging;

namespace Safir.EventSourcing
{
    [PublicAPI]
    public interface IEventMetadataProvider
    {
        string GetTypeDiscriminator<T>(T @event, int version) where T : IEvent;

        ValueTask<string> GetTypeDiscriminatorAsync<T>(T @event, int version, CancellationToken cancellationToken = default)
            where T : IEvent;

        Type GetType(string discriminator, int version);

        ValueTask<Type> GetTypeAsync(string discriminator, int version, CancellationToken cancellationToken = default);
    }
}
