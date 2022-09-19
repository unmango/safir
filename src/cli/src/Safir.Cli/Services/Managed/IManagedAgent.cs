using System;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Managed;

public interface IManagedAgent
{
    Task<Uri> StartAsync(
        Action<string>? onOutput = null,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken);
}
