using DotNet.Testcontainers.Builders;
using Safir.Agent.Fixture;

namespace Safir.Agent.EndToEndTests;

public abstract class AgentTestBase : IAsyncLifetime
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected AgentTestBase(AgentFixture fixture)
    {
        Container = new TestcontainersBuilder<SafirAgentContainer>()
            .WithConfiguration(new(fixture.AgentImage.FullName))
            .Build();
    }

    protected SafirAgentContainer Container { get; }

    public Task InitializeAsync() => Container.StartAsync();

    public Task DisposeAsync() => Container.DisposeAsync().AsTask();
}
