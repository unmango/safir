using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal record LocalDirectorySource(string Name, string SourceDirectory) :
        ServiceSourceBase(SourceType.LocalDirectory, Name),
        ILocalDirectorySource
    {
    }
}
