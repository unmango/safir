using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

internal sealed class DefaultHandlerBuilder : IIntermediateHandlerBuilder, IHandlerBuilder
{
    private readonly List<Action<InvocationContext, IConfigurationBuilder>> _configureHostConfigActions = new();
    private readonly List<Action<HandlerBuilderContext, IConfigurationBuilder>> _configureAppConfigActions = new();
    private readonly List<Action<HandlerBuilderContext, IServiceCollection>> _configureServicesActions = new();
    private CommandHandler? _handler;

    IIntermediateHandlerBuilder IIntermediateHandlerBuilder.ConfigureHostConfiguration(
        Action<InvocationContext, IConfigurationBuilder> configureDelegate)
    {
        _configureHostConfigActions.Add(configureDelegate);
        return this;
    }

    IHandlerBuilder IHandlerBuilder.ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate)
    {
        _configureHostConfigActions.Add(configureDelegate);
        return this;
    }

    IHandlerBuilder IHandlerBuilder.ConfigureAppConfiguration(
        Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _configureAppConfigActions.Add(configureDelegate);
        return this;
    }

    IIntermediateHandlerBuilder IIntermediateHandlerBuilder.ConfigureAppConfiguration(
        Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _configureAppConfigActions.Add(configureDelegate);
        return this;
    }

    public IHandlerBuilder ConfigureHandler(CommandHandler handler)
    {
        _handler = handler;
        return this;
    }

    IHandlerBuilder IHandlerBuilder.ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
    {
        _configureServicesActions.Add(configureDelegate);
        return this;
    }

    IIntermediateHandlerBuilder IIntermediateHandlerBuilder.ConfigureServices(
        Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
    {
        _configureServicesActions.Add(configureDelegate);
        return this;
    }

    public ICommandHandler Build()
        => new DelegateHandler(this, _handler ?? throw new InvalidOperationException("No handler configured"));

    private IServiceProvider BuildServiceProvider(InvocationContext invocationContext)
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

        return services.BuildServiceProvider();
    }

    private class DelegateHandler : ICommandHandler
    {
        private readonly DefaultHandlerBuilder _builder;
        private readonly CommandHandler _handler;
        private bool _invoked;

        public DelegateHandler(DefaultHandlerBuilder builder, CommandHandler handler)
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

            var services = _builder.BuildServiceProvider(context);
            return await _handler(context, services);
        }
    }
}
