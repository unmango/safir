using System.Collections.Generic;
using System.Linq;

namespace Cli.Services
{
    internal interface IService
    {
        // Default impl feels dirty... probably remove later
        IEnumerable<IServiceCommand> Commands => Enumerable.Empty<IServiceCommand>();
        
        string Name { get; }

        IEnumerable<IServiceSource> Sources { get; }
    }
}
