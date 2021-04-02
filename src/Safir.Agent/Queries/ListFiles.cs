using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using Safir.Agent.Domain;
using Safir.Agent.Protos;

namespace Safir.Agent.Queries
{
    internal record ListFilesRequest(string Root) : IRequest<ListFilesResponse>;

    internal record ListFilesResponse(IEnumerable<FileSystemEntry> Files)
    {
        public static readonly ListFilesResponse Empty = new(Enumerable.Empty<FileSystemEntry>());
    }

    [UsedImplicitly]
    internal class ListFilesHandler : RequestHandler<ListFilesRequest, ListFilesResponse>
    {
        private static readonly EnumerationOptions _enumerationOptions = new() {
            RecurseSubdirectories = true,
        };
        
        private readonly IDirectory _directory;
        private readonly IPath _path;
        private readonly ILogger<ListFilesHandler> _logger;
        
        public ListFilesHandler(IDirectory directory, IPath path, ILogger<ListFilesHandler> logger)
        {
            _directory = directory ?? throw new ArgumentNullException(nameof(directory));
            _path = path ?? throw new ArgumentNullException(nameof(path));
            _logger = logger;
        }
        
        protected override ListFilesResponse Handle(ListFilesRequest request)
        {
            var root = request.Root;
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
            var files = entries.Select(x => new FileSystemEntry {
                Path = _path.GetRelativePath(root, x),
            });
            
            return new ListFilesResponse(files);
        }
    }
}
