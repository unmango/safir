using JetBrains.Annotations;
using Safir.Agent.Client;

namespace Safir.Manager.Agents;

[PublicAPI]
public interface IAgent : IAgentClient
{
    string Name { get; }
}
