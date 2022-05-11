using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Safir.Cli;

internal static class Program
{
    private static async Task<int> Main(string[] args) => await CreateBuilder()
        .Build()
        .InvokeAsync(args);

    private static CommandLineBuilder CreateBuilder() => new CommandLineBuilder()
        .UseDefaults();
}
