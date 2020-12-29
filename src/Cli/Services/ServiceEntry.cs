namespace Cli.Services
{
    internal record ServiceEntry
    {
        public string Name { get; init; } = string.Empty;
        
        public ServiceSource Source { get; init; }
        
        public string? GitCloneUrl { get; init; }
        
        public ServiceType Type { get; init; }
    }
}
