using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public sealed class HandlerBuilder : IHandlerBuilder<HandlerBuilder>
{
    private readonly List<Action<InvocationContext, IConfigurationBuilder>> _configureHostConfigActions = new();
    private readonly List<Action<HandlerBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new();
    private readonly List<Action<HandlerBuilderContext, IServiceCollection>> _configureServicesActions = new();

    public HandlerBuilder ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate)
    {
        _configureHostConfigActions.Add(configureDelegate);
        return this;
    }

    public HandlerBuilder ConfigureAppConfiguration(Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _configureAppConfigActions.Add(configureDelegate);
        return this;
    }

    public HandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
    {
        _configureServicesActions.Add(configureDelegate);
        return this;
    }

    public ConfiguredHandlerBuilder ConfigureHandler(CommandHandler handler) => new(this, handler);

    internal IServiceProvider BuildServiceProvider(InvocationContext invocationContext)
    {
        var hostConfigBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection();

        foreach (var configure in _configureHostConfigActions)
            configure(invocationContext, hostConfigBuilder);

        var hostConfiguration = hostConfigBuilder.Build();

        var context = new HandlerBuilderContext(hostConfiguration, invocationContext);

        var appConfigBuilder = new ConfigurationBuilder()
            .AddConfiguration(hostConfiguration, shouldDisposeConfiguration: true);

        foreach (var configure in _configureAppConfigActions) {
            configure(context, appConfigBuilder);
        }

        var appConfiguration = appConfigBuilder.Build();
        context.Configuration = appConfiguration;

        var services = new ServiceCollection()
            .AddSingleton(context)
            .AddSingleton(_ => appConfiguration)
            .AddSingleton(invocationContext)
            .AddSingleton(invocationContext.BindingContext)
            .AddSingleton(invocationContext.Console)
            .AddTransient(_ => invocationContext.ParseResult);

        foreach (var configure in _configureServicesActions)
            configure(context, services);

        return services.BuildServiceProvider();
    }
}
