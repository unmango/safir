using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace Safir.Cli
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using var tokenSource = new CancellationTokenSource();

            try
            {
                var builder = CreateApplicationBuilder();

                // TODO

                var app = builder.Build();

                return await app.RunAsync(args, tokenSource.Token).ConfigureAwait(false);
            }
            catch (Exception)
            {
                tokenSource.Cancel();

                return 1;
            }
        }

        private static IApplicationBuilder CreateApplicationBuilder()
            => CommandLineApplication.CreateDefaultBuilder();
    }
}
