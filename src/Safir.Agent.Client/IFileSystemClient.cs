using System.Collections.Generic;
using System.Threading;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    public interface IFileSystemClient
    {
        IAsyncEnumerable<FileSystemEntry> ListAsync(CancellationToken cancellationToken = default);
    }
}
