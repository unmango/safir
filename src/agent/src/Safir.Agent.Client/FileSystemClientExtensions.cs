using System.Collections.Generic;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using JetBrains.Annotations;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public static class FileSystemClientExtensions
    {
        public static AsyncServerStreamingCall<FileSystemEntry> ListFiles(
            this FileSystem.FileSystemClient client,
            CancellationToken cancellationToken = default)
        {
            return client.ListFiles(new Empty(), Metadata.Empty, null, cancellationToken);
        }

        public static IAsyncEnumerable<FileSystemEntry> ListFilesAsync(
            this FileSystem.FileSystemClient client,
            CancellationToken cancellationToken = default)
        {
            var streamingCall = client.ListFiles(new Empty(), Metadata.Empty, null, cancellationToken);
            return streamingCall.ResponseStream.ReadAllAsync(cancellationToken);
        }
    }
}
