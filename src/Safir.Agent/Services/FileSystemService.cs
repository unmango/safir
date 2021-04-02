using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Protos;
using Safir.Agent.Queries;

namespace Safir.Agent.Services
{
    [UsedImplicitly]
    internal class FileSystemService : FileSystem.FileSystemBase
    {
        private readonly IOptions<AgentOptions> _options;
        private readonly ISender _sender;
        private readonly ILogger<FileSystemService> _logger;

        public FileSystemService(IOptions<AgentOptions> options, ISender sender, ILogger<FileSystemService> logger)
        {
            _options = options;
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger;
        }

        public override async Task List(
            Empty request,
            IServerStreamWriter<FileSystemEntry> responseStream,
            ServerCallContext context)
        {
            var root = _options.Value.DataDirectory;
            if (string.IsNullOrWhiteSpace(root))
            {
                _logger.LogInformation("No data directory set, returning");
                return;
            }
            
            _logger.LogTrace("Sending list files request");
            var result = await _sender.Send(new ListFilesRequest(root));
            _logger.LogTrace("Got list files response");

            if (!result.Files.Any()) return;

            _logger.LogTrace("Writing files to response stream");
            await responseStream.WriteAllAsync(result.Files);
        }
    }
}
