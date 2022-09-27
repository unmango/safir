using System.Diagnostics;
using JetBrains.Annotations;

namespace Safir.Rpc.Abstractions;

[PublicAPI]
public interface IManagedServiceProcess
{
    IObservable<string> Error { get; }

    IObservable<string> Output { get; }

    ValueTask<Process> StartAsync(IEnumerable<string>? args = null, CancellationToken cancellationToken = default);
}
