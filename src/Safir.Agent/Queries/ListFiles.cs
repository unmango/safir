using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Safir.Agent.Configuration;
using Safir.Agent.Domain;
using Safir.Agent.Protos;

namespace Safir.Agent.Queries
{
    internal record ListFilesRequest : IRequest<ListFilesResponse>;

    internal record ListFilesResponse(IEnumerable<FileSystemEntry> Files)
    {
        public static readonly ListFilesResponse Empty = new(Enumerable.Empty<FileSystemEntry>());
    }

    internal class ListFilesHandler : RequestHandler<ListFilesRequest, ListFilesResponse>
    {
        private static readonly EnumerationOptions _enumerationOptions = new() {
            RecurseSubdirectories = true,
        };
        
        private readonly IOptions<AgentOptions> _options;
        private readonly IDirectory _directory;
        private readonly ILogger<ListFilesHandler> _logger;
        
        public ListFilesHandler(
            IOptions<AgentOptions> options,
            IDirectory directory,
            ILogger<ListFilesHandler> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _logger = logger;
        }
        
        protected override ListFilesResponse Handle(ListFilesRequest request)
        {
            var root = _options.Value.DataDirectory;
            if (string.IsNullOrWhiteSpace(root))
            {
                _logger.LogDebug("No root configured");
                return ListFilesResponse.Empty;
            }

            if (!_directory.Exists(root))
            {
                _logger.LogError("Data directory doesn't exist");
                return ListFilesResponse.Empty;
            }

            _logger.LogTrace("Enumerating file system entries");
            var entries = _directory.EnumerateFileSystemEntries(root, "*", _enumerationOptions);
            
            _logger.LogTrace("Creating file response messages");
            var files = entries.Select(x => new FileSystemEntry { Path = x });
            
            return new ListFilesResponse(files);
        }
    }
}
