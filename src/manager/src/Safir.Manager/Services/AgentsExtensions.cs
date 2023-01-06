using Safir.Agent.Client;
using Safir.Agent.V1Alpha1;

namespace Safir.Manager.Services;

internal static class AgentsExtensions
{
    public static IAsyncEnumerable<(string Host, ListResponse Entry)> ListFilesAsync(
        this IAgents agents,
        CancellationToken cancellationToken)
        => agents.FileSystem.ToAsyncEnumerable().SelectMany(
            x => x.Value.ListAsync(cancellationToken),
            (pair, entry) => (pair.Key, entry));
}
