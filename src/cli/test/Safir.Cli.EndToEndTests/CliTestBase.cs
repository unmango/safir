using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Safir.Agent.Fixture;
using Safir.Manager.Fixture;

namespace Safir.Cli.EndToEndTests;

public abstract class CliTestBase : IAsyncLifetime
{
    protected const string AgentName = "CliE2E";
    protected const string ManagerName = "CliE2E";

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    protected CliTestBase(CliFixture fixture)
    {
        Network = new TestcontainersNetworkBuilder()
            .WithName("cli-e2e")
            .Build();

        AgentContainer = new TestcontainersBuilder<SafirAgentContainer>()
            .WithConfiguration(new(fixture.AgentImage.FullName))
            .WithHostname("agent")
            .WithNetwork(Network)
            .Build();

        ManagerContainer = new TestcontainersBuilder<SafirManagerContainer>()
            .WithConfiguration(new(fixture.ManagerImage.FullName))
            .WithAgent(AgentName, AgentContainer.InternalAddress.AbsoluteUri)
            .WithHostname("manager")
            .WithNetwork(Network)
            .Build();

        CliBuilder = new TestcontainersBuilder<CliContainer>()
            .WithImage(fixture.CliImage)
            .WithNetwork(Network);
    }

    protected IDockerNetwork Network { get; }

    protected SafirAgentContainer AgentContainer { get; }

    protected SafirManagerContainer ManagerContainer { get; }

    protected ITestcontainersBuilder<CliContainer> CliBuilder { get; }

    protected string ManagerConfig => $$"""
        {
          "Managers": [
            {
              "Name": "{{ManagerName}}",
              "Uri": "{{ManagerContainer.InternalAddress}}"
            }
          ]
        }
        """;

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
