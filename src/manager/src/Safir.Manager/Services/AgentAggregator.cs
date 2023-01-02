using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Safir.Agent.Client;
using Safir.Agent.Protos;
using Host = Safir.Protos.Host;

namespace Safir.Manager.Services;

internal sealed class AgentAggregator : IAgents
{
    public AgentAggregator(IOptions<ManagerConfiguration> options, GrpcClientFactory clientFactory)
    {
        var agents = options.Value.GetAgentOptions().Select(x => x.Name).ToList();

        FileSystem = agents.ToDictionary(x => x, clientFactory.CreateFileSystemClient);
        Host = agents.ToDictionary(x => x, clientFactory.CreateHostClient);
    }

    public IEnumerable<KeyValuePair<string, FileSystem.FileSystemClient>> FileSystem { get; }

    public IEnumerable<KeyValuePair<string, Host.HostClient>> Host { get; }
}
