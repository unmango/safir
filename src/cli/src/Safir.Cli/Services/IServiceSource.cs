using Safir.Cli.Services.Configuration;

namespace Safir.Cli.Services
{
    public interface IServiceSource
    {
        SourceType Type { get; }
        
        string Name { get; }
        
        int Priority { get; init; }
    }
}
