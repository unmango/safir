using System.Collections.Generic;
using System.Threading;
using Grpc.Core;
using JetBrains.Annotations;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public interface IFileSystemClient
    {
        AsyncServerStreamingCall<FileSystemEntry> List(CancellationToken cancellationToken = default);

        IAsyncEnumerable<FileSystemEntry> ListAsync(CancellationToken cancellationToken = default);
    }
}
