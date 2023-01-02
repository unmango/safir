using DotNet.Testcontainers.Builders;
using JetBrains.Annotations;

namespace Safir.Agent.Fixture;

[PublicAPI]
// ReSharper disable once IdentifierTypo
public static class TestcontainersBuilderExtensions
{
    public static ITestcontainersBuilder<T> WithDataDirectory<T>(
        this ITestcontainersBuilder<T> builder,
        string directory)
        where T : SafirAgentContainer
        => builder.WithEnvironment(SafirAgentConfiguration.DataDirectoryKey, directory);

    public static ITestcontainersBuilder<T> WithConfiguration<T>(
        this ITestcontainersBuilder<T> builder,
        SafirAgentConfiguration configuration)
        where T : SafirAgentContainer
    {
        builder = configuration.Environments.Aggregate(builder, (current, environment)
            => current.WithEnvironment(environment.Key, environment.Value));

        return builder
            .WithImage(configuration.Image)
            .WithExposedPort(configuration.DefaultPort)
            .WithPortBinding(configuration.Port, configuration.DefaultPort)
            .WithWaitStrategy(configuration.WaitStrategy)
            .ConfigureContainer(container => {
                container.ContainerPort = configuration.DefaultPort;
                container.DataDirectory = configuration.DataDirectory;
            });
    }
}
