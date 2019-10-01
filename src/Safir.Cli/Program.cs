using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using Safir.Cli.Commands;

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
                Add.Register(builder);
                
                var app = new CommandLineApplication(builder);

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
    }
}
