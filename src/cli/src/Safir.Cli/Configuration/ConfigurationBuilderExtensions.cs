using Microsoft.Extensions.Configuration;

namespace Safir.Cli.Configuration;

internal static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddDefaultUserConfigurationPaths(this IConfigurationBuilder builder)
    {
        var root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var directory = Path.Combine(root, "safir");
        var file = Path.Combine(directory, SafirDefaults.ConfigFileName);

        return builder.AddInMemoryCollection(new Dictionary<string, string?> {
            [SafirDefaults.ConfigDirectoryKey] = directory,
            [SafirDefaults.ConfigFileKey] = file,
        });
    }

    public static IConfigurationBuilder AddSafirCliDefault(this IConfigurationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .AddDefaultUserConfigurationPaths()
            .AddEnvironmentVariables("SAFIR_") // TODO: Don't use prefix when in docker container
            .Build();

        // Add first to allow config file to overwrite
        builder.AddConfiguration(configuration);

        var file = configuration.GetValue<string>(SafirDefaults.ConfigFileKey)
                   ?? throw new InvalidOperationException($"No value for {SafirDefaults.ConfigFileKey}");

        return builder.AddJsonFile(file, optional: true, reloadOnChange: true);
    }
}
