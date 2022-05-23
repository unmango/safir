using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public class ConfiguredHandlerBuilder : IHandlerBuilder
{
    private readonly HandlerBuilder _inner;
    private readonly CommandHandler _handler;

    internal ConfiguredHandlerBuilder(HandlerBuilder inner, CommandHandler handler)
    {
        _inner = inner;
        _handler = handler;
    }

    public ConfiguredHandlerBuilder ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate)
    {
        _inner.ConfigureHostConfiguration(configureDelegate);
        return this;
    }

    public ConfiguredHandlerBuilder ConfigureAppConfiguration(
        Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        _inner.ConfigureAppConfiguration(configureDelegate);
        return this;
    }

    public ConfiguredHandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
    {
        _inner.ConfigureServices(configureDelegate);
        return this;
    }

    public ConfiguredHandlerBuilder ConfigureHandler(CommandHandler handler) => new(_inner, handler);

    public ICommandHandler Build() => new DelegateHandler(_inner, _handler);

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

            var services = _builder.BuildServiceProvider(context);
            return await _handler(context, services);
        }
    }
}
