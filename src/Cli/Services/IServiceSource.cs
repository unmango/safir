using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services
{
    internal interface IServiceSource
    {
        ValueTask<CanInitializeResult> CanInitializeAsync(CancellationToken cancellationToken = default);

        Task InitializeAsync(
            ServiceEntry service,
            string workingDirectory,
            CancellationToken cancellationToken = default);

        ValueTask<bool> IsInitializedAsync(
            ServiceEntry service,
            string workingDirectory,
            CancellationToken cancellationToken = default);

        bool Satisfies(ServiceEntry service);
    }
}
