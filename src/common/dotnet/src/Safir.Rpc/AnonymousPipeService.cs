using System.Diagnostics;
using System.IO.Pipes;
using System.Reactive.Subjects;
using JetBrains.Annotations;
using Safir.Common;
using Safir.Rpc.Abstractions;

namespace Safir.Rpc;

[PublicAPI]
public abstract class AnonymousPipeService : IManagedService
{
    private readonly Subject<string> _error = new();
    private readonly Subject<string> _output = new();
    private Process? _process;
    private AnonymousPipeServerStream? _pipeServer;
    private Uri? _uri;

    public IObservable<string> Error => _error;

    public IObservable<string> Output => _output;

    public Uri Uri => _uri ?? throw new InvalidOperationException("Service must be started first");

    public async ValueTask StartAsync(IEnumerable<string>? args, CancellationToken cancellationToken = default)
    {
        var grpcPort = NetUtil.NextFreePort();
        _uri = new Uri($"http://127.0.0.1:{grpcPort}");

        _pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);

        var startupArgs = CreateStartupArgs(_uri.AbsoluteUri)
            .Concat(new[] { "--pipe-handle", _pipeServer.GetClientHandleAsString() });

        _process = await StartProcessAsync(startupArgs, _output.OnNext, _error.OnNext, cancellationToken);
    }

    public async ValueTask StopAsync(CancellationToken cancellationToken)
    {
        if (_process is null || _pipeServer is null) return;

        await using var writer = new StreamWriter(_pipeServer);

        await writer.WriteLineAsync("stop");
        await writer.FlushAsync();

        await _process.WaitForExitAsync(cancellationToken);
    }

    protected virtual IEnumerable<string> CreateStartupArgs(string url) => new[] { "--urls", Quote(url) };

    protected abstract Task<Process> StartProcessAsync(
        IEnumerable<string> args,
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken);

    private static string Quote(string value) => $"\"{value}\"";
}
