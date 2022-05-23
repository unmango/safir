using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class HandlerBuilder
{
    public static IIntermediateHandlerBuilder Create() => new DefaultHandlerBuilder();

    public static T ConfigureAppConfiguration<T>(this IHandlerBuilder<T> builder, Action<IConfigurationBuilder> configureDelegate)
        where T : IHandlerBuilder<T>
        => builder.ConfigureAppConfiguration((_, b) => configureDelegate(b));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Func<InvocationContext, IServiceProvider, Task<int>> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Func<IServiceProvider, Task<int>> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Func<InvocationContext, IServiceProvider, Task> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Func<IServiceProvider, Task> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Func<InvocationContext, IServiceProvider, int> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Func<IServiceProvider, int> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Action<InvocationContext, IServiceProvider> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler<T>(
        this IHandlerBuilder<T> builder,
        Action<IServiceProvider> handle)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static T ConfigureHostConfiguration<T>(
        this IHandlerBuilder<T> builder,
        Action<IConfigurationBuilder> configureDelegate)
        where T : IHandlerBuilder<T>
        => builder.ConfigureHostConfiguration((_, b) => configureDelegate(b));

    public static T ConfigureServices<T>(this IHandlerBuilder<T> builder, Action<IServiceCollection> configureDelegate)
        where T : IHandlerBuilder<T>
        => builder.ConfigureServices((_, s) => configureDelegate(s));
}
