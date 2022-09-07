using Microsoft.Extensions.Options;
using Safir.Agent.Client;
using Safir.Agent.Protos;
using Safir.Cli.Configuration;
using Safir.Protos;

namespace Safir.Cli.Services;

internal sealed class AgentClientManager : IAgents
{
    private readonly SafirOptions _options;

    public AgentClientManager(IOptions<SafirOptions> options)
    {
        _options = options.Value;
    }

    public IAgentClient GetAgent(string name)
    {
        throw new System.NotImplementedException();
    }
}
