using Grpc.Core;
using JetBrains.Annotations;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.Client;

[PublicAPI]
public interface IHostClient
{
    InfoResponse GetInfo(CancellationToken cancellationToken = default);

    AsyncUnaryCall<InfoResponse> GetInfoAsync(CancellationToken cancellationToken = default);
}
