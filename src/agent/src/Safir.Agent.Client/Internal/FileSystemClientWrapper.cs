using Grpc.Core;
using Safir.Agent.V1Alpha1;

namespace Safir.Agent.Client.Internal;

internal class FileSystemClientWrapper : IFileSystemClient
{
    private readonly FilesService.FilesServiceClient _client;

    public FileSystemClientWrapper(FilesService.FilesServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public AsyncServerStreamingCall<ListResponse> List(CancellationToken cancellationToken = default)
    {
        return _client.List(cancellationToken);
    }

    public IAsyncEnumerable<ListResponse> ListAsync(CancellationToken cancellationToken = default)
    {
        return _client.ListAsync(cancellationToken);
    }
}
