using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Safir.Cli.Services.Managed;

internal static class AgentUtil
{
    public static IEnumerable<string> CreateStartupArgs(string url) => new[] { "--urls", Quote(url) };

    public static async Task<string> GetProjectPathAsync()
    {
        var gitRoot = await Git.GetRootAsync();
        return Path.Combine(gitRoot, "src", "agent", "src", "Safir.Agent");
    }

    private static string Quote(string value) => $"\"{value}\"";
}
