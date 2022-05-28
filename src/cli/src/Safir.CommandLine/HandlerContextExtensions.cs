using System;
using System.CommandLine.Invocation;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class HandlerContextExtensions
{
    public static Task<int> ExecuteAsync<T>(this HandlerContext context)
        where T : ICommandHandler
        => context.GetRequiredService<T>().InvokeAsync(context.InvocationContext);

    public static Task<int> ExecuteAsync<T>(this HandlerContext context, Func<T, CancellationToken, Task<int>> handler)
        where T : notnull
        => handler(context.GetRequiredService<T>(), context.GetCancellationToken());

    public static CancellationToken GetCancellationToken(this HandlerContext context)
        => context.InvocationContext.GetCancellationToken();

    public static T GetRequiredService<T>(this HandlerContext context)
        where T : notnull
        => context.Services.GetRequiredService<T>();

    public static object GetService(this HandlerContext context, Type serviceType) => context.Services.GetService(serviceType);

    public static T? GetService<T>(this HandlerContext context) => context.Services.GetService<T>();
}
