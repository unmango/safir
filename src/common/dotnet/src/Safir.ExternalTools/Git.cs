using JetBrains.Annotations;

namespace Safir.ExternalTools;

[PublicAPI]
public static class Git
{
    public static async Task<string> GetRootAsync(string workingDirectory = "")
    {
        var (stdOut, stdErr) = await CliTool.RunAsync("git", "rev-parse --show-toplevel", workingDirectory);

        return string.IsNullOrWhiteSpace(stdErr)
            ? stdOut.Trim()
            : throw new InvalidOperationException(stdErr);
    }
}
