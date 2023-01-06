using Grpc.Core;
using JetBrains.Annotations;
using Safir.Agent.V1Alpha1;

namespace Safir.Agent.Client;

[PublicAPI]
public static class FileSystemClientExtensions
{
    public static AsyncServerStreamingCall<ListResponse> List(
        this FilesService.FilesServiceClient client,
        CancellationToken cancellationToken = default)
    {
        return client.List(new ListRequest(), Metadata.Empty, null, cancellationToken);
    }

    public static IAsyncEnumerable<ListResponse> ListAsync(
        this FilesService.FilesServiceClient client,
        CancellationToken cancellationToken = default)
    {
        var streamingCall = client.List(new ListRequest(), Metadata.Empty, null, cancellationToken);
        return streamingCall.ResponseStream.ReadAllAsync(cancellationToken);
    }
}
