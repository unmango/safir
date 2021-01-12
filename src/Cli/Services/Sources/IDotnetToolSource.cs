namespace Cli.Services.Sources
{
    internal interface IDotnetToolSource : IServiceSource
    {
        string ToolName { get; }
        
        string? ExtraArgs { get; }
    }
}
