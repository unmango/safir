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
        public static AsyncServerStreamingCall<FileSystemEntry> List(
            this FileSystem.FileSystemClient client,
            CancellationToken cancellationToken = default)
        {
            return client.List(new Empty(), Metadata.Empty, null, cancellationToken);
        }

        public static IAsyncEnumerable<FileSystemEntry> ListAsync(
            this FileSystem.FileSystemClient client,
            CancellationToken cancellationToken = default)
        {
            var streamingCall = client.List(new Empty(), Metadata.Empty, null, cancellationToken);
            return streamingCall.ResponseStream.ReadAllAsync(cancellationToken);
        }
    }
}
