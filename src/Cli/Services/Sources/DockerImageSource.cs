namespace Cli.Services.Sources
{
    internal record DockerImageSource(string ImageName, string? Tag = null)
    {
    }
}
