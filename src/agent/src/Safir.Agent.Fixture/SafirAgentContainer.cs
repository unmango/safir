using DotNet.Testcontainers.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Fixture;
using Safir.Agent.Protos;
using Safir.Protos;

namespace Safir.Agent.Fixture;

[PublicAPI]
public class SafirAgentContainer : SafirContainer
{
    protected SafirAgentContainer(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger) { }

    public string DataDirectory { get; internal set; } = SafirAgentConfiguration.DefaultDataDirectory;

    public Host.HostClient CreateHostClient() => new(CreateChannel());

    public FileSystem.FileSystemClient CreateFileSystemClient() => new(CreateChannel());

    public Task CreateMediaFileAsync(string file, ReadOnlyMemory<byte> content, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(DataDirectory, file);
        return CopyFileAsync(path, content.ToArray(), ct: cancellationToken);
    }

    public Task CreateMediaFileAsync(string file, CancellationToken cancellationToken = default)
        => CreateMediaFileAsync(file, ReadOnlyMemory<byte>.Empty, cancellationToken);
}
