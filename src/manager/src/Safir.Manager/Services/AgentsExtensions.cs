using Safir.Agent.Client;
using Safir.Agent.Protos;

namespace Safir.Manager.Services;

internal static class AgentsExtensions
{
    public static IAsyncEnumerable<(string Host, FileSystemEntry Entry)> ListFilesAsync(
        this IAgents agents,
        CancellationToken cancellationToken)
        => agents.FileSystem.ToAsyncEnumerable().SelectMany(
            x => x.Value.ListFilesAsync(cancellationToken),
            (pair, entry) => (pair.Key, entry));
}
