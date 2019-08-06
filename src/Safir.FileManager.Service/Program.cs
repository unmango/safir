using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Safir.FileManager.Service
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using var tokenSource = new CancellationTokenSource();

            var command = new RootCommand();

            try
            {
                var host = CreateHostBuilder(args).Build();

                await host.RunAsync(tokenSource.Token).ConfigureAwait(false);

                return 0;
            }
            catch (Exception)
            {
                tokenSource.Cancel();

                return 1;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<Worker>();
                });
    }
}
