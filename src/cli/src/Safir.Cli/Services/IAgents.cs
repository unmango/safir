using Safir.Agent.Client;

namespace Safir.Cli.Services;

public interface IAgents
{
    IAgentClient GetAgent(string name);
}
