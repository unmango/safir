using Safir.Cli.Services.Configuration;

namespace Safir.Cli.Services.Sources
{
    internal record DockerBuildSource(string Name, string BuildContext, string? Tag = null) :
        ServiceSourceBase(SourceType.DockerBuild, Name),
        IDockerBuildSource
    {
    }
}
