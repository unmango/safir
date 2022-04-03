using Safir.Cli.Services.Configuration;

namespace Safir.Cli.Services.Sources;

internal record DotnetToolSource(string Name, string ToolName, string? ExtraArgs = null) :
    ServiceSourceBase(SourceType.DotnetTool, Name),
    IDotnetToolSource
{
}