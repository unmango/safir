using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal record DockerBuildSource(string BuildContext, string? Tag = null) :
        ServiceSourceBase(SourceType.DockerBuild),
        IDockerBuildSource
    {
    }
}
