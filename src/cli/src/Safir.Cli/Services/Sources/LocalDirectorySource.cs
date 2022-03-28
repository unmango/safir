using Safir.Cli.Services.Configuration;

namespace Safir.Cli.Services.Sources
{
    internal record LocalDirectorySource(string Name, string SourceDirectory) :
        ServiceSourceBase(SourceType.LocalDirectory, Name),
        ILocalDirectorySource
    {
    }
}
