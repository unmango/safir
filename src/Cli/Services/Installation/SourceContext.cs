namespace Cli.Services.Installation
{
    internal interface ISourceContext
    {
        InstallationContext Parent { get; }
        
        IServiceSource Source { get; }
    }
    
    // TODO: Directory?
    internal record SourceContext<T>(InstallationContext Parent) : ISourceContext
        where T : IServiceSource
    {
        public T Source { get; init; }
        
        IServiceSource ISourceContext.Source => Source;
    }
}
