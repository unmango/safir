using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class HandlerBuilderExtensions
{
    public static IHandlerBuilder ConfigureAppConfiguration(
        this IHandlerBuilder builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureAppConfiguration((_, b) => configureDelegate(b));

    public static IHandlerBuilder ConfigureHostConfiguration(
        this IHandlerBuilder builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureHostConfiguration((_, b) => configureDelegate(b));

    public static IHandlerBuilder ConfigureServices(this IHandlerBuilder builder, Action<IServiceCollection> configureDelegate)
        => builder.ConfigureServices((_, s) => configureDelegate(s));
}
