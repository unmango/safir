namespace Safir.Rpc.Abstractions;

public interface IManagedService
{
    IObservable<string> Error { get; }

    IObservable<string> Output { get; }

    Uri Uri { get; }

    ValueTask StartAsync(IEnumerable<string>? args = null, CancellationToken cancellationToken = default);

    ValueTask StopAsync(CancellationToken cancellationToken);
}
