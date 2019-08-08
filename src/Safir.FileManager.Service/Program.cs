using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.FileManager.Service
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using var tokenSource = new CancellationTokenSource();

            try
            {
                var parser = CreateCommandLineBuilder().Build();

                return await parser.InvokeAsync(args).ConfigureAwait(false);
            }
            catch (Exception)
            {
                tokenSource.Cancel();

                return 1;
            }
        }

        public static CommandLineBuilder CreateCommandLineBuilder()
            => new CommandLineBuilder()
                .UseDefaults()
                .UseRestApi();
    }
}
