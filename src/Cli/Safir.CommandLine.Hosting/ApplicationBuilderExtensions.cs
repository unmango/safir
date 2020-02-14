using System;
using System.CommandLine;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;

namespace Safir.CommandLine.Hosting
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHost(
            this IApplicationBuilder builder,
            Func<string[], IHostBuilder> hostBuilderFactory,
            Action<IHostBuilder>? configureHost = null)
        {
            builder.ParserBuilder.UseHost(hostBuilderFactory, configureHost);

            return builder;
        }

        public static IApplicationBuilder UseHost(
            this IApplicationBuilder builder,
            Action<IHostBuilder>? configureHost = null)
        {
            builder.ParserBuilder.UseHost(configureHost);

            return builder;
        }
    }
}
