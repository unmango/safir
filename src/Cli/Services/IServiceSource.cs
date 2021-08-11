using Cli.Services.Configuration;

namespace Cli.Services
{
    public interface IServiceSource
    {
        SourceType Type { get; }
        
        int Priority { get; init; }
        
        static abstract string Name { get; }
    }
}
