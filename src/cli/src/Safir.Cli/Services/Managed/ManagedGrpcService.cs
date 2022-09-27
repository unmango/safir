using System.Diagnostics;
using System.Reactive.Subjects;
using Grpc.Net.Client;
using Safir.Common;
using Safir.Protos;
using Safir.Rpc.Abstractions;

namespace Safir.Cli.Services.Managed;

internal sealed class ManagedGrpcService : IManagedService
{
    private readonly IManagedServiceProcess _managedServiceProcess;
    private readonly Subject<string> _error = new();
    private readonly Subject<string> _output = new();
    private readonly IList<IDisposable> _subscriptions = new List<IDisposable>();
    private Uri? _uri;
    private Process? _process;

    public ManagedGrpcService(IManagedServiceProcess managedServiceProcess)
    {
        _managedServiceProcess = managedServiceProcess;
    }

    public IObservable<string> Error => _error;

    public IObservable<string> Output => _output;

    public Uri Uri => _uri ?? throw new InvalidOperationException("Service must be started first");

    public async ValueTask StartAsync(IEnumerable<string>? args = null, CancellationToken cancellationToken = default)
    {
        var httpPort = NetUtil.NextFreePort();
        var httpUri = new Uri($"http://127.0.0.1:{httpPort}");

        var grpcPort = NetUtil.NextFreePort();
        _uri = new Uri($"http://127.0.0.1:{grpcPort}");

        args = (args ?? Enumerable.Empty<string>()).Concat(new[] {
            "--no-launch-profile",
            "--Kestrel:Endpoints:WebApi:Protocols", "Http1",
            "--Kestrel:Endpoints:WebApi:Url", httpUri.AbsoluteUri,
            "--Kestrel:Endpoints:Grpc:Protocols", "Http2",
            "--Kestrel:Endpoints:Grpc:Url", _uri.AbsoluteUri,
        });

        _subscriptions.Add(_managedServiceProcess.Error.Subscribe(_error));
        _subscriptions.Add(_managedServiceProcess.Output.Subscribe(_output));

        _process = await _managedServiceProcess.StartAsync(args, cancellationToken);
    }

    public async ValueTask StopAsync(CancellationToken cancellationToken)
    {
        if (_uri is null || _process is null)
            return;

        var channel = GrpcChannel.ForAddress(_uri);
        var client = new Host.HostClient(channel);

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(10));

        await client.StopAsync(new(), cancellationToken: cts.Token);
        await _process.WaitForExitAsync(cts.Token);

        foreach (var subscription in _subscriptions)
            subscription.Dispose();
    }
}
