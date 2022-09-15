using System.Linq;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Safir.Agent.Client;
using Safir.Agent.Protos;
using Safir.Cli.Configuration;
using Safir.Protos;

namespace Safir.Cli.Services;

internal sealed class AgentClientManager : IAgents
{
    private readonly IOptionsMonitor<SafirOptions> _options;
    private readonly GrpcClientFactory _clientFactory;

    public AgentClientManager(IOptionsMonitor<SafirOptions> options, GrpcClientFactory clientFactory)
    {
        _options = options;
        _clientFactory = clientFactory;
    }

    public bool ShouldStartManagedAgent => !_options.CurrentValue.Agents?.Any() ?? true;

    public IAgentClient GetAgent(string name) => _clientFactory.CreateAgentClient(name);

    public ManagedAgent CreateManagedAgent()
    {
        return new DevelopmentAgent();
    }
}
