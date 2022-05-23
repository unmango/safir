using System;
using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public sealed class HandlerBuilder : IIntermediateHandlerBuilder
{
    private readonly IIntermediateHandlerBuilder _inner = new DefaultHandlerBuilder();

    public IIntermediateHandlerBuilder ConfigureHostConfiguration(
        Action<InvocationContext, IConfigurationBuilder> configureDelegate)
        => _inner.ConfigureHostConfiguration(configureDelegate);

    public IIntermediateHandlerBuilder ConfigureAppConfiguration(
        Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
        => _inner.ConfigureAppConfiguration(configureDelegate);

    public IHandlerBuilder ConfigureHandler(CommandHandler handler) => _inner.ConfigureHandler(handler);

    public IIntermediateHandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
        => _inner.ConfigureServices(configureDelegate);

    public static IIntermediateHandlerBuilder Create() => new DefaultHandlerBuilder();
}
