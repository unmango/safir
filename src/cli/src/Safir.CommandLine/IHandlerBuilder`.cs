using System;
using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public interface IHandlerBuilder<out T>
{
    T ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate);

    T ConfigureAppConfiguration(Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate);

    ConfiguredHandlerBuilder ConfigureHandler(CommandHandler handler);

    T ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate);
}
