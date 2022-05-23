using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

[PublicAPI]
public static class HandlerBuilderExtensions
{
    public static T ConfigureAppConfiguration<T>(this IHandlerBuilder<T> builder, Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureAppConfiguration((_, b) => configureDelegate(b));

    public static T ConfigureHostConfiguration<T>(
        this IHandlerBuilder<T> builder,
        Action<IConfigurationBuilder> configureDelegate)
        => builder.ConfigureHostConfiguration((_, b) => configureDelegate(b));

    public static T ConfigureServices<T>(this IHandlerBuilder<T> builder, Action<IServiceCollection> configureDelegate)
        => builder.ConfigureServices((_, s) => configureDelegate(s));
}
