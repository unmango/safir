using Grpc.Core;
using JetBrains.Annotations;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.Client;

[PublicAPI]
public static class HostClientExtensions
{
    public static InfoResponse GetInfo(this HostService.HostServiceClient client, CancellationToken cancellationToken = default)
    {
        return client.Info(new InfoRequest(), Metadata.Empty, null, cancellationToken);
    }

    public static AsyncUnaryCall<InfoResponse> GetInfoAsync(
        this HostService.HostServiceClient client,
        CancellationToken cancellationToken = default)
    {
        return client.InfoAsync(new InfoRequest(), Metadata.Empty, null, cancellationToken);
    }
}
