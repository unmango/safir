using System.CommandLine.Builder;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class CommandLineBuilderExtensions
{
    public static CommandLineBuilder UseHandlerLifetime(this CommandLineBuilder builder)
        => builder.AddMiddleware(async (context, next) => {
            if (!context.TryGetHandlerContext(out var handlerContext))
                return;

            var lifetime = handlerContext.Services.GetService<IHandlerLifetime>();

            if (lifetime is null)
                throw new InvalidOperationException("Handler lifetime must be registered to use handler lifetime");

            var cancellationToken = context.GetCancellationToken();

            await lifetime.WaitForStartAsync(cancellationToken);
            await next(context);
            await lifetime.StopAsync(cancellationToken);
        });
}
