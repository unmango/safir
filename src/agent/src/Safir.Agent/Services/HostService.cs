using System.Net;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Safir.Protos;
using Host = Safir.Protos.Host;

namespace Safir.Agent.Services;

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
