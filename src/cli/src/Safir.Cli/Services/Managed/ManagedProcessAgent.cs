using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Managed;

internal abstract class ManagedProcessAgent : IManagedAgent, IAsyncDisposable, IDisposable
{
    private Process? _process;
    private AnonymousPipeServerStream? _pipeServer;

    public async Task<Uri> StartAsync(
        Action<string>? onOutput = null,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        var grpcPort = NetUtil.NextFreePort();
        var uri = new Uri($"http://127.0.0.1:{grpcPort}");

        _pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);

        var args = AgentUtil.CreateStartupArgs(uri.AbsoluteUri)
            .Concat(new[] { "--pipe-handle", _pipeServer.GetClientHandleAsString() });

        _process = await StartProcessAsync(
            args,
            onOutput ?? (_ => { }),
            onError ?? (_ => { }),
            cancellationToken);

        return uri;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_process is null || _pipeServer is null) return;

        await using var writer = new StreamWriter(_pipeServer);

        await writer.WriteLineAsync("stop");
        await writer.FlushAsync();

        await _process.WaitForExitAsync(cancellationToken);
    }

    protected abstract Task<Process> StartProcessAsync(
        IEnumerable<string> args,
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken);

    public ValueTask DisposeAsync()
    {
        return _pipeServer?.DisposeAsync() ?? new ValueTask();
    }

    public void Dispose()
    {
        _pipeServer?.Dispose();
    }
}
