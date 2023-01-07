using Grpc.Core;
using JetBrains.Annotations;
using Safir.Agent.V1Alpha1;

namespace Safir.Agent.Client;

[PublicAPI]
public interface IFileSystemClient
{
    AsyncServerStreamingCall<ListResponse> List(CancellationToken cancellationToken = default);

    IAsyncEnumerable<ListResponse> ListAsync(CancellationToken cancellationToken = default);
}
