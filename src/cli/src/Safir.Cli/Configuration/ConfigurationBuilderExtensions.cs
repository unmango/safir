using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Safir.Cli.Configuration;

internal static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddDefaultUserProfileDirectory(this IConfigurationBuilder builder)
    {
        var directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        return builder.AddInMemoryCollection(new Dictionary<string, string> {
            [SafirDefaults.ConfigDirectoryKey] = directory,
        });
    }

    public static IConfigurationBuilder AddSafirCliDefault(this IConfigurationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddDefaultUserProfileDirectory()
            .AddEnvironmentVariables()
            .Build();

        // Potentially inefficient, but allows sharing the config file logic
        var options = new SafirOptions();
        configuration.Bind(options);
        var file = options.Config.File;

        builder.AddJsonFile(file, optional: true, reloadOnChange: true);

        return builder.AddConfiguration(configuration);
    }
}
