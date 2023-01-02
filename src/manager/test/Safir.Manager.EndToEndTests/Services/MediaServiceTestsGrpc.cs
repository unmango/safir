using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Networks;
using Google.Protobuf.WellKnownTypes;
using Safir.Agent.Client;
using Safir.Agent.Fixture;
using Safir.EndToEndTesting;
using Safir.Grpc;
using Safir.Manager.Fixture;
using Safir.XUnit.AspNetCore;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests.Services;

// [Collection(ManagerServiceCollection.Name)]
[Trait("Category", "EndToEnd")]
public class MediaServiceTestsGrpc : IClassFixture<ManagerFixture>, IAsyncLifetime
{
    private readonly IDockerNetwork _network;
    private readonly SafirAgentContainer _agentContainer;
    private readonly SafirManagerContainer _managerContainer;

    public MediaServiceTestsGrpc(ManagerFixture fixture, ITestOutputHelper output)
    {
        TestcontainersSettings.Logger = new TestOutputHelperLogger(output);

        _network = new TestcontainersNetworkBuilder()
            .WithName("manager-e2e")
            .Build();

        _agentContainer = new TestcontainersBuilder<SafirAgentContainer>()
            .WithConfiguration(new(fixture.AgentImage.FullName))
            .WithHostname("agent")
            .WithNetwork(_network)
            .Build();

        _managerContainer = new TestcontainersBuilder<SafirManagerContainer>()
            .WithConfiguration(new(fixture.ManagerImage.FullName))
            .WithAgent("Test", _agentContainer.InternalAddress.AbsoluteUri)
            .WithNetwork(_network)
            .WithTestOutputHelper(output)
            .Build();
    }

    [Fact]
    public async Task List_ReturnsTestData()
    {
        const string fileName = "Test.txt";
        await _agentContainer.CreateMediaFileAsync(fileName);

        var result = await _managerContainer.CreateMediaClient()
            .List(new Empty())
            .ResponseStream
            .ToListAsync();

        var item = Assert.Single(result);
        Assert.Equal("Test", item.Host);
        Assert.Equal(fileName, item.Path);
    }

    public async Task InitializeAsync()
    {
        await _network.CreateAsync();

        await Task.WhenAll(
            _agentContainer.StartAsync(),
            _managerContainer.StartAsync());
    }

    public async Task DisposeAsync()
    {
        await _managerContainer.DisposeAsync();
        await _agentContainer.DisposeAsync();
        await _network.DeleteAsync();
    }
}
