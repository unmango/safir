using System.Net;
using Grpc.Core;
using Safir.Common.V1Alpha1;

namespace Safir.Manager.Services;

internal sealed class HostGrpcService : HostService.HostServiceBase
{
    public override Task<InfoResponse> Info(InfoRequest request, ServerCallContext context)
    {
        return Task.FromResult(new InfoResponse {
            MachineName = Environment.MachineName,
            HostName = Dns.GetHostName(),
        });
    }
}
