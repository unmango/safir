using System;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

public class HandlerBuilder<T> : IHandlerBuilder
{
    private readonly IHandlerBuilder _builder;

    public HandlerBuilder(IHandlerBuilder builder)
    {
        _builder = builder;
    }

    public IHandlerBuilder ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate)
    {
        return _builder.ConfigureHostConfiguration(configureDelegate);
    }

    public IHandlerBuilder ConfigureAppConfiguration(Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate)
    {
        return _builder.ConfigureAppConfiguration(configureDelegate);
    }

    public IHandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate)
    {
        return _builder.ConfigureServices(configureDelegate);
    }

    public ICommandHandler Build()
    {
        IHandlerBuilderCreator<T> creator = new TestImpl();

        return _builder.Build();
    }

    IHandlerBuilder IHandlerBuilder.ConfigureHandler(CommandHandler handler)
    {
        return _builder.ConfigureHandler(handler);
    }
}
