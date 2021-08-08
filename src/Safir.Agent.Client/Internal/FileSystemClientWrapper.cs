using System;
using System.Collections.Generic;
using System.Threading;
using Grpc.Core;
using Safir.Agent.Protos;

namespace Safir.Agent.Client.Internal
{
    internal class FileSystemClientWrapper : IFileSystemClient
    {
        private readonly FileSystem.FileSystemClient _client;

        public FileSystemClientWrapper(FileSystem.FileSystemClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public AsyncServerStreamingCall<FileSystemEntry> List(CancellationToken cancellationToken = default)
        {
            return _client.List(cancellationToken);
        }

        public IAsyncEnumerable<FileSystemEntry> ListAsync(CancellationToken cancellationToken = default)
        {
            return _client.ListAsync(cancellationToken);
        }
    }
}
