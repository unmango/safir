using System.Collections.Generic;

namespace Safir.Cli.Services;

internal interface IServiceRegistry : IReadOnlyDictionary<string, IService>
{
    IEnumerable<IService> Services { get; }
}