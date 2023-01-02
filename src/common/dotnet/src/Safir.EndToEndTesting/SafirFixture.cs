using DotNet.Testcontainers.Images;
using JetBrains.Annotations;
using Xunit;

namespace Safir.EndToEndTesting;

[PublicAPI]
public abstract class SafirFixture : IAsyncLifetime
{
    protected SafirFixture(string tagPrefix)
    {
        CommonImage = Image(SafirImageBuilder.DefaultCommonImageName, tagPrefix);
        AgentImage = Image(SafirImageBuilder.DefaultAgentImageName, tagPrefix);
        ManagerImage = Image(SafirImageBuilder.DefaultManagerImageName, tagPrefix);
    }

    public IDockerImage CommonImage { get; }

    public IDockerImage AgentImage { get; }

    public IDockerImage ManagerImage { get; }

    public async Task InitializeAsync()
    {
        _ = await SafirImageBuilder.CreateCommon()
            .WithName(CommonImage)
            .Build();

        var agentBuild = SafirImageBuilder.CreateAgent(CommonImage)
            .WithName(AgentImage)
            .Build();

        var managerBuild = SafirImageBuilder.CreateManager(CommonImage)
            .WithName(ManagerImage)
            .Build();

        _ = await Task.WhenAll(agentBuild, managerBuild);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private static IDockerImage Image(string image, string tagPrefix)
        => new DockerImage($"{image}:{tagPrefix}-{SafirImageBuilder.DefaultTag}");
}
