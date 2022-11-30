using System.Collections.Immutable;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using JetBrains.Annotations;
using Xunit.Abstractions;

namespace Safir.EndToEndTesting;

[PublicAPI]
public delegate ServiceTestConfiguration ConfigureServiceTest(ServiceTestConfiguration configuration);

public delegate ITestcontainersBuilder<TestcontainersContainer> ConfigureBuilder(
    ITestcontainersBuilder<TestcontainersContainer> builder);

[PublicAPI]
public record ServiceTestConfiguration(
    ServiceFixtureBase Fixture,
    ITestOutputHelper OutputHelper,
    ImmutableList<ConfigureBuilder> ConfigureActions)
{
    public static readonly ConfigureServiceTest NoOp = x => x;

    public ServiceTestConfiguration WithEnvironment(string key, string value)
        => WithConfigureAction(x => x.WithEnvironment(key, value));

    public ServiceTestConfiguration WithEnvironment(IReadOnlyDictionary<string, string> values)
        => WithConfigureAction(x => x.WithEnvironment(values));

    internal ServiceTestConfiguration WithConfigureAction(ConfigureBuilder configure)
        => this with {
            ConfigureActions = ConfigureActions.Add(configure),
        };
}
