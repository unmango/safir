using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Safir.Agent.Fixture;
using Safir.Manager.Fixture;

namespace Safir.Manager.EndToEndTests;

public abstract class ManagerTestBase : IAsyncLifetime
{
    protected const string AgentName = "ManagerE2E";

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected ManagerTestBase(ManagerFixture fixture)
    {
        Network = new TestcontainersNetworkBuilder()
            .WithName("manager-e2e")
            .Build();

        AgentContainer = new TestcontainersBuilder<SafirAgentContainer>()
            .WithConfiguration(new(fixture.AgentImage.FullName))
            .WithHostname("agent")
            .WithNetwork(Network)
            .Build();

        ManagerContainer = new TestcontainersBuilder<SafirManagerContainer>()
            .WithConfiguration(new(fixture.ManagerImage.FullName))
            .WithAgent(AgentName, AgentContainer.InternalAddress.AbsoluteUri)
            .WithNetwork(Network)
            .Build();
    }

    protected IDockerNetwork Network { get; }

    protected SafirAgentContainer AgentContainer { get; }

    protected SafirManagerContainer ManagerContainer { get; }

    public async Task InitializeAsync()
    {
        await Network.CreateAsync();

        await Task.WhenAll(
            AgentContainer.StartAsync(),
            ManagerContainer.StartAsync());
    }

    public async Task DisposeAsync()
    {
        await ManagerContainer.DisposeAsync();
        await AgentContainer.DisposeAsync();
        await Network.DeleteAsync();
    }
}
