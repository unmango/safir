using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal record DotnetToolSource(string Name, string ToolName, string? ExtraArgs = null) :
        ServiceSourceBase(SourceType.DotnetTool, Name),
        IDotnetToolSource
    {
    }
}
