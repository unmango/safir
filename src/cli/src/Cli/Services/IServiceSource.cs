using Cli.Services.Configuration;

namespace Cli.Services
{
    public interface IServiceSource
    {
        SourceType Type { get; }
        
        string Name { get; }
        
        int Priority { get; init; }
    }
}
