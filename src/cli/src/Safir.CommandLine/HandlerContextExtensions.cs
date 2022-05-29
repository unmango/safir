using System;
using System.CommandLine.Parsing;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class HandlerContextExtensions
{
    public static CancellationToken GetCancellationToken(this HandlerContext context)
        => context.InvocationContext.GetCancellationToken();

    public static ParseResult GetParseResult(this HandlerContext context) => context.InvocationContext.ParseResult;

    public static T GetRequiredService<T>(this HandlerContext context)
        where T : notnull
        => context.Services.GetRequiredService<T>();

    public static object GetService(this HandlerContext context, Type serviceType) => context.Services.GetService(serviceType);

    public static T? GetService<T>(this HandlerContext context) => context.Services.GetService<T>();
}
