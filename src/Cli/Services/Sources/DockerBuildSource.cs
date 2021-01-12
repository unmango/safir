using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal record DockerBuildSource(string Name, string BuildContext, string? Tag = null) :
        ServiceSourceBase(SourceType.DockerBuild, Name),
        IDockerBuildSource
    {
    }
}
