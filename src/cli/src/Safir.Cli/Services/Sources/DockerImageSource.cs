using Safir.Cli.Services.Configuration;

namespace Safir.Cli.Services.Sources;

internal record DockerImageSource(string Name, string ImageName, string? Tag = null) :
    ServiceSourceBase(SourceType.DockerImage, Name),
    IDockerImageSource
{
}