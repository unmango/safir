using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal abstract record ServiceSourceBase : IServiceSource
    {
        public ServiceSourceBase(SourceType type, int priority = default)
        {
            Type = type;
            Priority = priority;
        }
        
        public SourceType Type { get; }
        
        public int Priority { get; init; }
    }
}
