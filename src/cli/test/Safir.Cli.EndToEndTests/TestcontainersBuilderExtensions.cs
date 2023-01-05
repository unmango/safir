using System.Text;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Safir.Cli.EndToEndTests;

// ReSharper disable once IdentifierTypo
internal static class TestcontainersBuilderExtensions
{
    private const string DefaultConfigFile = "/config/config.json";

    public static async Task<ExecResult> ExecAsync<T>(this ITestcontainersBuilder<T> builder, params string[] command)
        where T : CliContainer
    {
        builder = builder.WithEntrypoint("tail", "-f", "/dev/null");

        var container = builder.Build();
        await container.StartAsync();
        var result = await container.ExecAsync(command);
        await container.StopAsync();

        return result;
    }

    public static Task<ExecResult> ExecCliAsync<T>(this ITestcontainersBuilder<T> builder, params string[] command)
        where T : CliContainer
        => builder.ExecAsync(new[] { "dotnet", "Safir.Cli.dll" }.Concat(command).ToArray());

    public static ITestcontainersBuilder<T> WithConfigurationFile<T>(this ITestcontainersBuilder<T> builder, string file)
        where T : CliContainer
        => builder.WithEnvironment("SAFIR_CONFIG__FILE", file);

    public static ITestcontainersBuilder<T> WithConfiguration<T>(
        this ITestcontainersBuilder<T> builder,
        string file,
        string configuration)
        where T : CliContainer
        => builder.WithStartupCallback(async (container, cancellationToken) => {
            await container.CopyFileAsync(file, Encoding.UTF8.GetBytes(configuration), ct: cancellationToken);
        });

    public static ITestcontainersBuilder<T> WithConfiguration<T>(this ITestcontainersBuilder<T> builder, string configuration)
        where T : CliContainer
        => builder
            .WithConfigurationFile(DefaultConfigFile)
            .WithConfiguration(DefaultConfigFile, configuration);
}
