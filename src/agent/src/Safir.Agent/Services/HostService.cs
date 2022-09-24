using System.Net;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Protos;
using Host = Safir.Protos.Host; // This was breaking the CI docker build for some reason...

namespace Safir.Agent.Services;

internal class HostService : Host.HostBase
{
    private readonly IOptions<AgentOptions> _options;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public HostService(IOptions<AgentOptions> options, IHostApplicationLifetime applicationLifetime)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _applicationLifetime = applicationLifetime;
    }

    public override Task<HostInfo> GetInfo(Empty request, ServerCallContext context)
    {
        return Task.FromResult(new HostInfo {
            MachineName = Environment.MachineName,
            HostName = Dns.GetHostName(),
        });
    }

    public override Task<Empty> Stop(Empty request, ServerCallContext context)
    {
        _applicationLifetime.StopApplication();
        return Task.FromResult(new Empty());
    }
}
