namespace Cli.Services.Installation
{
    internal interface ISourceContext
    {
        Operation Operation { get; }
        
        InstallationContext Parent { get; }
        
        IServiceSource Source { get; }
    }
    
    // TODO: Directory?
    internal record SourceContext<T> : ISourceContext
        where T : IServiceSource
    {
        public SourceContext(InstallationContext parent, T source)
        {
            Operation = operation;
            Parent = parent;
            Source = source;
        }

        public Operation Operation { get; init; }
        
        public InstallationContext Parent { get; init; }
        
        public T Source { get; init; }
        
        IServiceSource ISourceContext.Source => Source;
    }
}
