namespace Cli.Services.Sources
{
    internal interface IDockerImageSource : IServiceSource
    {
        string ImageName { get; }
        
        string? Tag { get; }
    }
}
