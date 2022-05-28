using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
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
    private CommandHandler? _handler;

    public ICommandHandler Build()
        => new DelegateHandler(this, _handler ?? throw new InvalidOperationException("No handler configured"));

    public IHandlerBuilder ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate)
    {
        _configureHostConfigActions.Add(configureDelegate);
        return this;
    }

    public IHandlerBuilder ConfigureAppConfiguration(Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _configureAppConfigActions.Add(configureDelegate);
        return this;
    }

    public IHandlerBuilder ConfigureHandler(CommandHandler handler)
    {
        _handler = handler;
        return this;
    }

    public IHandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
    {
        _configureServicesActions.Add(configureDelegate);
        return this;
    }

    public static IHandlerBuilder Create() => new HandlerBuilder();

    private HandlerContext BuildHandlerContext(InvocationContext invocationContext)
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

        // Stolen from HostBuilder and host middleware implementations
        var services = new ServiceCollection()
            .AddSingleton(context)
            .AddSingleton(_ => appConfiguration)
            .AddSingleton(invocationContext)
            .AddSingleton(invocationContext.BindingContext)
            .AddSingleton(invocationContext.Console)
            .AddTransient(_ => invocationContext.ParseResult);

        foreach (var configure in _configureServicesActions)
            configure(context, services);

        return new(appConfiguration, invocationContext, services.BuildServiceProvider());
    }

    private class DelegateHandler : ICommandHandler
    {
        private readonly HandlerBuilder _builder;
        private readonly CommandHandler _handler;
        private bool _invoked;

        public DelegateHandler(HandlerBuilder builder, CommandHandler handler)
        {
            _builder = builder;
            _handler = handler;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            // Naive guard against building the provider twice
            if (_invoked)
                throw new InvalidOperationException("Handler can only be invoked once");

            _invoked = true;

            var handlerContext = _builder.BuildHandlerContext(context);
            return await _handler(handlerContext);
        }
    }
}
