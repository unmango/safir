using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Safir.Cli.DependencyInjection;

namespace Safir.Cli.Commands;

internal sealed class CommandBuilder
{
    private readonly List<Action<IConfiguration, IServiceCollection>> _configureServices = new();
    private readonly List<Action<IConfigurationBuilder>> _configure = new();

    private CommandBuilder() { }

    public static CommandBuilder Create() => new();

    public CommandBuilder Configure(Action<IConfigurationBuilder> configure)
    {
        _configure.Add(configure);

        return this;
    }

    public CommandBuilder ConfigureServices(Action<IConfiguration, IServiceCollection> configure)
    {
        _configureServices.Add(configure);

        return this;
    }

    public IServiceCollection Build()
    {
        var builder = new ConfigurationBuilder();
        foreach (var configure in _configure) {
            configure(builder);
        }

        var configuration = builder.Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(_ => configuration);

        foreach (var configure in _configureServices) {
            configure(configuration, services);
        }

        return services;
    }
}
