using System;
using System.Threading.Tasks;

namespace Safir.Cli.Services;

internal static class Git
{
    public static async Task<string> GetRootAsync(string workingDirectory = "")
    {
        var (stdOut, stdErr) = await CliTool.RunAsync("git", "rev-parse --show-toplevel", workingDirectory);

        return string.IsNullOrWhiteSpace(stdErr)
            ? stdOut.Trim()
            : throw new InvalidOperationException(stdErr);
    }
}
