using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using JetBrains.Annotations;
using Safir.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public static class HostClientExtensions
    {
        public static HostInfo GetInfo(this Host.HostClient client, CancellationToken cancellationToken = default)
        {
            return client.GetInfo(new Empty(), Metadata.Empty, null, cancellationToken);
        }
        
        public static AsyncUnaryCall<HostInfo> GetInfoAsync(
            this Host.HostClient client,
            CancellationToken cancellationToken = default)
        {
            return client.GetInfoAsync(new Empty(), Metadata.Empty, null, cancellationToken);
        }
    }
}
