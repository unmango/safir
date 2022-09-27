using Safir.Rpc.Abstractions;

namespace Safir.Cli.Services.Managed;

internal sealed class DefaultManagedAgent : IManagedAgent
{
    private readonly IManagedService _service = new ManagedGrpcService(
        new DevelopmentManagedServiceProcess(Path.Combine("src", "agent", "src", "Safir.Agent")));

    public IObservable<string> Error => _service.Error;

    public IObservable<string> Output => _service.Output;

    public Uri Uri => _service.Uri;

    public ValueTask StartAsync(IEnumerable<string>? args = null, CancellationToken cancellationToken = default)
        => _service.StartAsync(args, cancellationToken);

    public ValueTask StopAsync(CancellationToken cancellationToken)
        => _service.StopAsync(cancellationToken);
}
