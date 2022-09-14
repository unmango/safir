using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

internal static class CliTool
{
    public static async Task<(string stdOut, string stdErr)> RunAsync(string filename, string args, string workingDirectory)
    {
        var process = new Process {
            StartInfo = new(filename, args) {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
            },
        };

        _ = process.Start();

        var readOutput = process.StandardOutput.ReadToEndAsync();
        var readError = process.StandardError.ReadToEndAsync();

        return await Task.WhenAll(readOutput, readError) switch {
            [ { } stdOut, { } stdErr, ..] => (stdOut, stdErr),
            [ { } stdOut, ..] => (stdOut, string.Empty),
            _ => throw new NotSupportedException("Unexpected array length"),
        };
    }

    public static Task RunAsync(
        string filename,
        string args,
        Action<string> onOutput,
        CancellationToken cancellationToken = default)
        => RunAsync(filename, args, string.Empty, onOutput, null, cancellationToken);

    public static Task RunAsync(
        string filename,
        string args,
        string workingDirectory,
        Action<string> onOutput,
        CancellationToken cancellationToken = default)
        => RunAsync(filename, args, workingDirectory, onOutput, null, cancellationToken);

    public static Task RunAsync(
        string filename,
        string args,
        string workingDirectory,
        Action<string> onOutput,
        Action<string>? onError = null,
        CancellationToken cancellationToken = default)
    {
        var process = new Process {
            StartInfo = new(filename, args) {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardError = onError is not null,
                RedirectStandardOutput = true,
            },
            EnableRaisingEvents = true,
        };

        process.OutputDataReceived += (_, e) => {
            if (!string.IsNullOrWhiteSpace(e.Data))
                onOutput(e.Data);
        };

        if (onError is not null) {
            process.ErrorDataReceived += (_, e) => {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    onError(e.Data);
            };
        }

        _ = process.Start();

        process.BeginOutputReadLine();

        if (onError is not null)
            process.BeginErrorReadLine();

        return process.WaitForExitAsync(cancellationToken);
    }
}
