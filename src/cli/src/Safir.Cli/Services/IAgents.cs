using Safir.Agent.Client;
using Safir.Cli.Services.Managed;

namespace Safir.Cli.Services;

public interface IAgents
{
    bool ShouldStartManagedAgent { get; }

    IAgentClient? GetAgent(string name);

    IManagedAgent CreateManagedAgent();
}
