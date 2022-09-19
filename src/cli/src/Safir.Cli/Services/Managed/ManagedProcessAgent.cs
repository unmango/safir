using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Managed;

internal abstract class ManagedProcessAgent : IManagedAgent
{
    private Process? _process;

    public async Task<Uri> StartAsync(
        Action<string>? onOutput = null,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        var grpcPort = NetUtil.NextFreePort();
        var uri = new Uri($"http://127.0.0.1:{grpcPort}");

        _process = await StartProcessAsync(
            uri.AbsoluteUri,
            onOutput ?? (_ => { }),
            onError ?? (_ => { }),
            cancellationToken);

        return uri;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_process is null) return;

        // await _process.StandardInput.WriteLineAsync("\x3");
        // _process.StandardInput.Close();
        _process.Kill();
        await _process.WaitForExitAsync(cancellationToken);
    }

    protected abstract Task<Process> StartProcessAsync(
        string url,
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken);
}
