using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public sealed class HandlerBuilder : IHandlerBuilder
{
    private readonly List<Action<InvocationContext, IConfigurationBuilder>> _configureHostConfigActions = new();
    private readonly List<Action<HandlerBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new();
    private readonly List<Action<HandlerBuilderContext, IServiceCollection>> _configureServicesActions = new();

    public HandlerContext Build(InvocationContext context) => BuildHandlerContext(context);

    public IHandlerBuilder ConfigureAppConfiguration(Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _configureAppConfigActions.Add(configureDelegate);
        return this;
    }

    public IHandlerBuilder ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate)
    {
        _configureHostConfigActions.Add(configureDelegate);
        return this;
    }

    public IHandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
    {
        _configureServicesActions.Add(configureDelegate);
        return this;
    }

    public static IHandlerBuilder Create() => new HandlerBuilder();

    private HandlerContext BuildHandlerContext(InvocationContext context)
    {
        var hostConfigBuilder = new ConfigurationBuilder()
            .AddInMemoryCollection();

        foreach (var configure in _configureHostConfigActions)
            configure(context, hostConfigBuilder);

        var hostConfiguration = hostConfigBuilder.Build();

        var builderContext = new HandlerBuilderContext(hostConfiguration, context);

        var appConfigBuilder = new ConfigurationBuilder()
            .AddConfiguration(hostConfiguration, shouldDisposeConfiguration: true);

        foreach (var configure in _configureAppConfigActions)
            configure(builderContext, appConfigBuilder);

        var appConfiguration = appConfigBuilder.Build();
        builderContext.Configuration = appConfiguration;

        // Stolen from HostBuilder and host middleware implementations
        var services = new ServiceCollection()
            .AddSingleton(builderContext)
            .AddSingleton<IConfiguration>(_ => appConfiguration)
            .AddSingleton(context)
            .AddSingleton(context.BindingContext)
            .AddSingleton(context.Console)
            .AddTransient(_ => context.ParseResult);

        foreach (var configure in _configureServicesActions)
            configure(builderContext, services);

        return new(appConfiguration, context, services.BuildServiceProvider());
    }
}
