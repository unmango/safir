using Cli.Services.Configuration;

namespace Cli.Services.Sources
{
    internal abstract record ServiceSourceBase : IServiceSource
    {
        public ServiceSourceBase(SourceType type, string name, int priority = default)
        {
            Type = type;
            Name = name;
            Priority = priority;
        }
        
        public SourceType Type { get; }
        
        public string Name { get; }
        
        public int Priority { get; init; }
    }
}
