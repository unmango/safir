using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Safir.Agent.Protos;
using Host = Safir.Protos.Host;

namespace Safir.Manager.Services;

internal sealed class AgentAggregator : IAgents
{
    public AgentAggregator(IOptions<ManagerConfiguration> options, GrpcClientFactory clientFactory)
    {
        FileSystem = CreateMap<FileSystem.FileSystemClient>(options, clientFactory);
        Host = CreateMap<Host.HostClient>(options, clientFactory);
    }

    public IEnumerable<KeyValuePair<string, FileSystem.FileSystemClient>> FileSystem { get; }

    public IEnumerable<KeyValuePair<string, Host.HostClient>> Host { get; }

    private static IEnumerable<KeyValuePair<string, T>> CreateMap<T>(
        IOptions<ManagerConfiguration> options,
        GrpcClientFactory clientFactory)
        where T : class
        => options.Value.GetAgentOptions()
            .Select(x => x.Name)
            .Select(x => new KeyValuePair<string, T>(x, clientFactory.CreateClient<T>(x)));
}
