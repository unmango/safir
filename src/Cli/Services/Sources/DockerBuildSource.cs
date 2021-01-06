namespace Cli.Services.Sources
{
    internal record DockerBuildSource(string BuildContext, string? Tag = null)
    {
    }
}
