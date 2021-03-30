using System;
using System.Net;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Protos;

namespace Safir.Agent.Services
{
    internal class HostService : Host.HostBase
    {
        private readonly IOptions<AgentOptions> _options;

        public HostService(IOptions<AgentOptions> options)
        {
            _options = options;
        }
        
        public override Task<HostInfo> GetInfo(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new HostInfo {
                MachineName = Environment.MachineName,
                HostName = Dns.GetHostName(),
                DataDirectory = _options.Value.DataDirectory,
                MaxDepth = _options.Value.MaxDepth,
            });
        }
    }
}
