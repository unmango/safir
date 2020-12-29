using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services
{
    internal interface IServiceRegistry
    {
        IReadOnlyList<ServiceEntry> Services { get; }

        Task<IService> GetServiceAsync(ServiceEntry service, CancellationToken cancellationToken = default);
    }
}
