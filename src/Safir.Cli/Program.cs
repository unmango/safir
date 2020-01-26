using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.Threading;
using System.Threading.Tasks;
using Safir.Cli.Commands.Add;

namespace Safir.Cli
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using var tokenSource = new CancellationTokenSource();

            try
            {
                var builder = CreateCommandLineBuilder();

                RegisterCommands(builder);

                var app = new CommandLineApplication(null!, builder.Build());

                return await app.RunAsync(args, tokenSource.Token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                tokenSource.Cancel();

                return 1;
            }
        }

        private static CommandLineBuilder CreateCommandLineBuilder()
            => new CommandLineBuilder().UseDefaults();

        private static void RegisterCommands(CommandLineBuilder builder)
        {
            // AddCommand.Register(builder);
        }
    }
}
