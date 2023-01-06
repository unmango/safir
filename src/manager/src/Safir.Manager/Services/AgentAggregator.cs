using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Safir.Agent.Client;
using Safir.Agent.V1Alpha1;

namespace Safir.Manager.Services;

internal sealed class AgentAggregator : IAgents
{
    public AgentAggregator(IOptions<ManagerConfiguration> options, GrpcClientFactory clientFactory)
    {
        var agents = options.Value.GetAgentOptions().Select(x => x.Name).ToList();

        FileSystem = agents.ToDictionary(x => x, clientFactory.CreateFileSystemClient);
        Host = agents.ToDictionary(x => x, clientFactory.CreateHostClient);
    }

    public IEnumerable<KeyValuePair<string, FilesService.FilesServiceClient>> FileSystem { get; }

    public IEnumerable<KeyValuePair<string, Common.V1Alpha1.HostService.HostServiceClient>> Host { get; }
}
