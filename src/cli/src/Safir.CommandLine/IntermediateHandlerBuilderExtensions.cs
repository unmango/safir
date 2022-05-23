using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class IntermediateHandlerBuilderExtensions
{
    public static IIntermediateHandlerBuilder ConfigureAppConfiguration(
        this IIntermediateHandlerBuilder builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureAppConfiguration((_, b) => configureDelegate(b));

    public static IHandlerBuilder ConfigureHandler(
        this IIntermediateHandlerBuilder builder,
        Func<InvocationContext, IServiceProvider, Task<int>> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IIntermediateHandlerBuilder builder,
        Func<IServiceProvider, Task<int>> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IIntermediateHandlerBuilder builder,
        Func<InvocationContext, IServiceProvider, Task> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IIntermediateHandlerBuilder builder,
        Func<IServiceProvider, Task> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IIntermediateHandlerBuilder builder,
        Func<InvocationContext, IServiceProvider, int> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IIntermediateHandlerBuilder builder,
        Func<IServiceProvider, int> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(
        this IIntermediateHandlerBuilder builder,
        Action<InvocationContext, IServiceProvider> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IHandlerBuilder ConfigureHandler(this IIntermediateHandlerBuilder builder, Action<IServiceProvider> handle)
        => builder.ConfigureHandler(HandlerDelegate.Create(handle));

    public static IIntermediateHandlerBuilder ConfigureHostConfiguration(
        this IIntermediateHandlerBuilder builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureHostConfiguration((_, b) => configureDelegate(b));

    public static IIntermediateHandlerBuilder ConfigureServices(
        this IIntermediateHandlerBuilder builder,
        Action<IServiceCollection> configureDelegate)
        => builder.ConfigureServices((_, s) => configureDelegate(s));
}
