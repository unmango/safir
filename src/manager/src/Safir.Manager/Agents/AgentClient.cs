using Safir.Agent.Client;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Manager.Agents;

internal class AgentClient : IAgent
{
    private readonly IAgentClient _client;

    public AgentClient(string name, IAgentClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public FileSystem.FileSystemClient FileSystem => _client.FileSystem;

    public Host.HostClient Host => _client.Host;

    public string Name { get; }
}
