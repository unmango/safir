using System.IO.Abstractions;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Safir.Agent.V1Alpha1;
using Safir.Grpc;

namespace Safir.Agent.Services;

internal sealed class FilesGrpcService : FilesService.FilesServiceBase
{
    private readonly IOptions<AgentConfiguration> _options;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<FilesGrpcService> _logger;

    public FilesGrpcService(
        IOptions<AgentConfiguration> options,
        IFileSystem fileSystem,
        ILogger<FilesGrpcService> logger)
    {
        _options = options;
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public override async Task List(
        ListRequest request,
        IServerStreamWriter<ListResponse> responseStream,
        ServerCallContext context)
    {
        var root = _options.Parse().DataDirectory;
        if (string.IsNullOrWhiteSpace(root)) {
            _logger.LogInformation("No data directory set, returning");
            return;
        }

        if (!_fileSystem.Directory.Exists(root)) {
            _logger.LogError("Data directory doesn't exist");
            return;
        }

        _logger.LogTrace("Enumerating file system entries");
        var entries = _fileSystem.Directory.EnumerateFileSystemEntries(root, "*");

        _logger.LogTrace("Creating file response messages");
        var files = entries.Select(x => new ListResponse {
            Path = _fileSystem.Path.GetRelativePath(root, x),
        });

        _logger.LogTrace("Writing files to response stream");
        await responseStream.WriteAllAsync(files);
    }
}
