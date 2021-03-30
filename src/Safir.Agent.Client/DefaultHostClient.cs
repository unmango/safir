using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    internal class DefaultHostClient : IHostClient
    {
        private readonly Host.HostClient _client;
        private readonly ILogger<DefaultHostClient> _logger;

        public DefaultHostClient(Host.HostClient client, ILogger<DefaultHostClient> logger)
        {
            _client = client;
            _logger = logger;
        }
        
        public async Task<HostInfo> GetHostInfoAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Requesting host info from agent");
            return await _client.GetInfoAsync(new Empty(), null, null, cancellationToken);
        }
    }
}
