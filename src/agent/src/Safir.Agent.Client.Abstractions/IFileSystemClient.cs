using Grpc.Core;
using JetBrains.Annotations;
using Safir.Agent.V1Alpha1;

namespace Safir.Agent.Client;

[PublicAPI]
public interface IFileSystemClient
{
    AsyncServerStreamingCall<FilesServiceListResponse> List(CancellationToken cancellationToken = default);

    IAsyncEnumerable<FilesServiceListResponse> ListAsync(CancellationToken cancellationToken = default);
}
