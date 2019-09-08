using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Safir.FileManager.Service
{
    internal static class RestExtensions
    {
        private static readonly Option _option = new Option("--rest", "Start the REST api");

        public static T UseRestApi<T>(this T builder)
            where T : CommandLineBuilder
        {
            builder
                .AddOption(_option)
                .UseMiddleware(RestApiMiddleware);

            return builder;
        }

        private static Task RestApiMiddleware(InvocationContext context, Func<InvocationContext, Task> next)
        {
            if (!context.ParseResult.HasOption(_option))
            {
                return next(context);
            }

            var args = context.ParseResult.UnmatchedTokens.ToArray();
            var hostBuilder = Rest.Program.CreateHostBuilder(args);

            using var host = hostBuilder
                .UseInvocationLifetime(context)
                .Build();

            return RunAsync(host, context, next);
        }

        private static async Task RunAsync(
            IHost host,
            InvocationContext context,
            Func<InvocationContext, Task> next)
        {
            var cancellationToken = context.GetCancellationToken();

            await host.StartAsync(cancellationToken).ConfigureAwait(false);

            await next(context).ConfigureAwait(false);

            await host.WaitForShutdownAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
