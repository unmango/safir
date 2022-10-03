using System.CommandLine.Invocation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public interface IHandlerBuilder
{
    IHandlerBuilder ConfigureHostConfiguration(Action<InvocationContext, IConfigurationBuilder> configureDelegate);

    IHandlerBuilder ConfigureAppConfiguration(Action<HandlerBuilderContext, IConfigurationBuilder> configureDelegate);

    IHandlerBuilder ConfigureServices(Action<HandlerBuilderContext, IServiceCollection> configureDelegate);

    HandlerContext Build(InvocationContext context);
}
