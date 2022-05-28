using System;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class HandlerBuilderExtensions
{
    public static IHandlerBuilder ConfigureAppConfiguration(
        this IHandlerBuilder builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureAppConfiguration((_, b) => configureDelegate(b));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder builder,
        Func<T, ParseResult, CancellationToken, Task<int>> handler)
        where T : notnull
        => builder.ConfigureHandler(HandlerDelegate.Create(handler));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder builder,
        Func<T, ParseResult, CancellationToken, Task> handle)
        where T : notnull
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(this IHandlerBuilder builder, Func<T, ParseResult, int> handle)
        where T : notnull
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(this IHandlerBuilder builder, Action<T, ParseResult> handle)
        where T : notnull
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHostConfiguration(
        this IHandlerBuilder builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureHostConfiguration((_, b) => configureDelegate(b));

    public static IHandlerBuilder ConfigureServices(this IHandlerBuilder builder, Action<IServiceCollection> configureDelegate)
        => builder.ConfigureServices((_, s) => configureDelegate(s));
}
