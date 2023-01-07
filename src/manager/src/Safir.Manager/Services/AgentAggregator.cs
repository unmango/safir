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
    }

    public IEnumerable<KeyValuePair<string, FilesService.FilesServiceClient>> FileSystem { get; }
}
