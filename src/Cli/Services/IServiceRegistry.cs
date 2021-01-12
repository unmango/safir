using System.Collections.Generic;

namespace Cli.Services
{
    internal interface IServiceRegistry : IReadOnlyDictionary<string, IService>
    {
        IEnumerable<IService> Services { get; }
    }
}
