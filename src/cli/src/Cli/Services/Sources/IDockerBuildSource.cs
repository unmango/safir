namespace Cli.Services.Sources
{
    internal interface IDockerBuildSource : IServiceSource
    {
        string BuildContext { get; }
        
        string? Tag { get; }
    }
}
