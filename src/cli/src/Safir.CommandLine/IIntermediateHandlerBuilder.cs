using System;
using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public interface IIntermediateHandlerBuilder
{
    IIntermediateHandlerBuilder ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate);

    IIntermediateHandlerBuilder ConfigureAppConfiguration(Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate);

    IHandlerBuilder ConfigureHandler(CommandHandler handler);

    IIntermediateHandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate);
}
