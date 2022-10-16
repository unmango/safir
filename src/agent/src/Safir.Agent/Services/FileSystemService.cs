using System.IO.Abstractions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Safir.Agent.Protos;
using Safir.Grpc;
using FileSystem = Safir.Agent.Protos.FileSystem;

namespace Safir.Agent.Services;

internal sealed class FileSystemService : FileSystem.FileSystemBase
{
    private readonly AgentOptions _options;
    private readonly IFileSystem _fileSystem;
    private readonly ILogger<FileSystemService> _logger;

    public FileSystemService(
        IOptions<AgentConfiguration> options,
        IFileSystem fileSystem,
        ILogger<FileSystemService> logger)
    {
        _options = options.Parse();
        _fileSystem = fileSystem;
        _logger = logger;
    }

    public override async Task ListFiles(
        Empty request,
        IServerStreamWriter<FileSystemEntry> responseStream,
        ServerCallContext context)
    {
        var root = _options.DataDirectory;
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
        var files = entries.Select(x => new FileSystemEntry {
            Path = _fileSystem.Path.GetRelativePath(root, x),
        });

        _logger.LogTrace("Writing files to response stream");
        await responseStream.WriteAllAsync(files);
    }
}
