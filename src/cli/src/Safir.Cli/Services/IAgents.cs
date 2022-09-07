namespace Safir.Cli.Services;

public interface IAgents : IAgent
{
    IAgent GetAgent(string name);
}
