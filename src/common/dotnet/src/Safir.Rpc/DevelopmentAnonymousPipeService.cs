using System.Diagnostics;
using JetBrains.Annotations;
using Safir.ExternalTools;

namespace Safir.Rpc;

[PublicAPI]
public sealed class DevelopmentAnonymousPipeService : AnonymousPipeService
{
    private readonly string _relativePath;

    public DevelopmentAnonymousPipeService(string relativePath)
    {
        _relativePath = relativePath;
    }

    protected override async Task<Process> StartProcessAsync(
        IEnumerable<string> args,
        Action<string> onOutput,
        Action<string> onError,
        CancellationToken cancellationToken)
    {
        var appStarted = new TaskCompletionSource();
        var gitRoot = await Git.GetRootAsync();
        var projectPath = Path.Combine(gitRoot, _relativePath);

        var process = Dotnet.Run(
            projectPath,
            // "Release",
            "Debug",
            processArgs: args,
            onOutput: data => {
                if (data.Contains("Now listening")) {
                    appStarted.SetResult();
                }

                onOutput(data);
            },
            onError: onError);

        var winner = await Task.WhenAny(
            Task.Delay(TimeSpan.FromSeconds(10), cancellationToken),
            appStarted.Task);

        if (winner != appStarted.Task)
            throw new InvalidOperationException("Agent didn't start in the allotted time");

        return process;
    }
}
