namespace Safir.Cli.Services.Sources;

internal interface ILocalDirectorySource : IServiceSource
{
    string SourceDirectory { get; }
}