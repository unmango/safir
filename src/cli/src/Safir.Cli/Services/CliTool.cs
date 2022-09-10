using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

public class CliTool
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

        try {
            _ = process.Start();
        }
        catch (Win32Exception) {
            throw new NotSupportedException($"'{filename}' was not found in PATH");
        }

        var readOutput = process.StandardOutput.ReadToEndAsync();
        var readError = process.StandardError.ReadToEndAsync();

        return await Task.WhenAll(readOutput, readError) switch {
            [ { } stdOut, { } stdErr, ..] => (stdOut, stdErr),
            [ { } stdOut, ..] => (stdOut, string.Empty),
            _ => throw new NotSupportedException("Unexpected array length"),
        };
    }
}
