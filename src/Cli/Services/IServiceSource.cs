using Cli.Internal;
using Cli.Services.Configuration;

namespace Cli.Services
{
    public interface IServiceSource : IPriority
    {
        SourceType Type { get; }
        
        string Name { get; }
    }
}
