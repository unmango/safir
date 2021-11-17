using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal record GitSource(string Name, string CloneUrl) :
        ServiceSourceBase(SourceType.Git, Name),
        IGitSource
    {
    }
}
