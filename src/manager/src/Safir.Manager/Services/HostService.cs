using System.Net;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Safir.Protos;
using Host = Safir.Protos.Host; // This was breaking the CI docker build for some reason...

namespace Safir.Manager.Services;

internal sealed class HostService : Host.HostBase
{
    public override Task<HostInfo> GetInfo(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new HostInfo {
            MachineName = Environment.MachineName,
            HostName = Dns.GetHostName(),
        });
    }
}
