using Safir.Agent.Client;

namespace Safir.Cli.Services;

public interface IAgents
{
    bool ShouldStartManagedAgent { get; }

    IAgentClient? GetAgent(string name);

    ManagedAgent CreateManagedAgent();
}
