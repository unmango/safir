using System;
using System.CommandLine.Invocation;
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

    public static IHandlerBuilder ConfigureHandler(
        this IHandlerBuilder builder,
        Func<InvocationContext, IServiceProvider, Task<int>> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IHandlerBuilder builder,
        Func<IServiceProvider, Task<int>> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IHandlerBuilder builder,
        Func<InvocationContext, IServiceProvider, Task> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IHandlerBuilder builder,
        Func<IServiceProvider, Task> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IHandlerBuilder builder,
        Func<InvocationContext, IServiceProvider, int> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IHandlerBuilder builder,
        Func<IServiceProvider, int> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IHandlerBuilder builder,
        Action<InvocationContext, IServiceProvider> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(this IHandlerBuilder builder, Action<IServiceProvider> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHostConfiguration(
        this IHandlerBuilder builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureHostConfiguration((_, b) => configureDelegate(b));

    public static IHandlerBuilder ConfigureServices(this IHandlerBuilder builder, Action<IServiceCollection> configureDelegate)
        => builder.ConfigureServices((_, s) => configureDelegate(s));
}
