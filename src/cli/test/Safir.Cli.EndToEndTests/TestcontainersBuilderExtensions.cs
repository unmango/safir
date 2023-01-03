using System.Text;
using DotNet.Testcontainers.Builders;

namespace Safir.Cli.EndToEndTests;

// ReSharper disable once IdentifierTypo
internal static class TestcontainersBuilderExtensions
{
    public static async Task<string> ExecAsync<T>(this ITestcontainersBuilder<T> builder, params string[] command)
        where T : CliContainer
    {
        builder = builder.WithEntrypoint("tail", "-f", "/dev/null");

        var container = builder.Build();
        await container.StartAsync();
        var result = await container.ExecAsync(command);
        await container.StopAsync();

        if (result.ExitCode > 0)
            throw new Exception(result.Stderr);

        return result.Stdout;
    }

    public static ITestcontainersBuilder<T> WithConfiguration<T>(
        this ITestcontainersBuilder<T> builder,
        string file,
        string configuration)
        where T : CliContainer
        => builder
            .WithEnvironment("SAFIR_CONFIG__FILE", file)
            .WithStartupCallback(async (container, cancellationToken) => {
                await container.CopyFileAsync(file, Encoding.UTF8.GetBytes(configuration), ct: cancellationToken);
            });
}
