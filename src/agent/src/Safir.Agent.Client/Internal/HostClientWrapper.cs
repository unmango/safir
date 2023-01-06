using Grpc.Core;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.Client.Internal;

internal class HostClientWrapper : IHostClient
{
    private readonly HostService.HostServiceClient _client;

    public HostClientWrapper(HostService.HostServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public InfoResponse GetInfo(CancellationToken cancellationToken = default)
    {
        return _client.GetInfo(cancellationToken);
    }

    public AsyncUnaryCall<InfoResponse> GetInfoAsync(CancellationToken cancellationToken = default)
    {
        return _client.GetInfoAsync(cancellationToken);
    }
}
