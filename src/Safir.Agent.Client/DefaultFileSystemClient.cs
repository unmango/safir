using System;
using System.Collections.Generic;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;

namespace Safir.Agent.Client
{
    internal class DefaultFileSystemClient : IFileSystemClient
    {
        private readonly FileSystem.FileSystemClient _client;
        private readonly ILogger<DefaultFileSystemClient> _logger;

        public DefaultFileSystemClient(FileSystem.FileSystemClient client, ILogger<DefaultFileSystemClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger;
        }
        
        public IAsyncEnumerable<FileSystemEntry> ListAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogTrace("Making request to list files from agent");
            var streamingCall = _client.List(new Empty(), null, null, cancellationToken);
            _logger.LogDebug("Received response from agent");
            _logger.LogTrace("Reading response stream as an async enumerable");
            return streamingCall.ResponseStream.ReadAllAsync(cancellationToken);
        }
    }
}
