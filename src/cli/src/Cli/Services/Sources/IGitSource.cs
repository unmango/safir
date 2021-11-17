namespace Cli.Services.Sources
{
    internal interface IGitSource : IServiceSource
    {
        string CloneUrl { get; }
    }
}
