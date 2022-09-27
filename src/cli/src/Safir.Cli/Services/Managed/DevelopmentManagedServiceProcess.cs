using System.Diagnostics;
using System.Reactive.Subjects;
using Safir.ExternalTools;
using Safir.Rpc.Abstractions;

namespace Safir.Cli.Services.Managed;

internal sealed class DevelopmentManagedServiceProcess : IManagedServiceProcess
{
    private readonly string _relativePath;
    private readonly Subject<string> _error = new();
    private readonly Subject<string> _output = new();

    public DevelopmentManagedServiceProcess(string relativePath)
    {
        _relativePath = relativePath;
    }

    public IObservable<string> Error => _error;

    public IObservable<string> Output => _output;

    public async ValueTask<Process> StartAsync(IEnumerable<string>? args = null, CancellationToken cancellationToken = default)
    {
        var appStarted = new TaskCompletionSource();
        using var subscription = _output.Subscribe(data => {
            if (data.Contains("Now listening")) {
                appStarted.SetResult();
            }
        });

        var gitRoot = await Git.GetRootAsync();
        var projectPath = Path.Combine(gitRoot, _relativePath);

        var process = Dotnet.Run(
            projectPath,
            "Release",
            processArgs: args,
            onOutput: _output.OnNext,
            onError: _error.OnNext);

        var winner = await Task.WhenAny(
            Task.Delay(TimeSpan.FromSeconds(10), cancellationToken),
            appStarted.Task);

        if (winner != appStarted.Task)
            throw new InvalidOperationException("Service didn't start in the allotted time");

        return process;
    }
}
