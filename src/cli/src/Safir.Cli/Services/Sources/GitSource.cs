using Safir.Cli.Services.Configuration;

namespace Safir.Cli.Services.Sources;

internal record GitSource(string Name, string CloneUrl) :
    ServiceSourceBase(SourceType.Git, Name),
    IGitSource;
