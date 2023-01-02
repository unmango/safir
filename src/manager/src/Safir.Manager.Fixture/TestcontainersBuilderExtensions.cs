using DotNet.Testcontainers.Builders;
using JetBrains.Annotations;

namespace Safir.Manager.Fixture;

[PublicAPI]
// ReSharper disable once IdentifierTypo
public static class TestcontainersBuilderExtensions
{
    public static ITestcontainersBuilder<T> WithAgent<T>(this ITestcontainersBuilder<T> builder, string name, string uri)
        where T : SafirManagerContainer
        => builder.WithEnvironment($"Agents:{name}:Uri", uri);

    public static ITestcontainersBuilder<T> WithConfiguration<T>(
        this ITestcontainersBuilder<T> builder,
        SafirManagerConfiguration configuration)
        where T : SafirManagerContainer
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
            });
    }
}
