using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Core.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using Safir.Agent.Protos;
using Safir.Agent.Queries;

namespace Safir.Agent.Services
{
    internal class FileSystemService : FileSystem.FileSystemBase
    {
        private readonly ISender _sender;
        private readonly ILogger<FileSystemService> _logger;

        public FileSystemService(ISender sender, ILogger<FileSystemService> logger)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger;
        }

        public override async Task List(
            Empty request,
            IServerStreamWriter<FileSystemEntry> responseStream,
            ServerCallContext context)
        {
            _logger.LogTrace("Sending list files request");
            var result = await _sender.Send(new ListFilesRequest());
            _logger.LogTrace("Got list files response");

            if (!result.Files.Any()) return;

            _logger.LogTrace("Writing files to response stream");
            await responseStream.WriteAllAsync(result.Files);
        }
    }
}
