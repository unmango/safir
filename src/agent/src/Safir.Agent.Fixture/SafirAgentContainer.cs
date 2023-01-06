using DotNet.Testcontainers.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Safir.Fixture;
using Safir.Agent.V1Alpha1;
using Safir.Common.V1Alpha1;

namespace Safir.Agent.Fixture;

[PublicAPI]
public class SafirAgentContainer : SafirContainer
{
    protected SafirAgentContainer(ITestcontainersConfiguration configuration, ILogger logger)
        : base(configuration, logger) { }

    public string DataDirectory { get; internal set; } = SafirAgentConfiguration.DefaultDataDirectory;

    public HostService.HostServiceClient CreateHostClient() => new(CreateChannel());

    public FilesService.FilesServiceClient CreateFileSystemClient() => new(CreateChannel());

    public Task CreateMediaFileAsync(string file, byte[] content, CancellationToken cancellationToken = default)
    {
        var path = Path.Combine(DataDirectory, file);
        return CopyFileAsync(path, content, ct: cancellationToken);
    }

    public Task CreateMediaFileAsync(string file, CancellationToken cancellationToken = default)
        => CreateMediaFileAsync(file, Array.Empty<byte>(), cancellationToken);
}
