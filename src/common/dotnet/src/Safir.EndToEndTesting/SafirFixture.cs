using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Images;
using JetBrains.Annotations;
using Safir.XUnit.AspNetCore;
using Xunit;
using Xunit.Abstractions;

namespace Safir.EndToEndTesting;

[PublicAPI]
public abstract class SafirFixture : IAsyncLifetime
{
    private readonly string _tagSuffix;

    protected SafirFixture(IMessageSink sink, string tagSuffix)
    {
        TestcontainersSettings.Logger = new MessageSinkLogger(sink);
        _tagSuffix = tagSuffix;
        CommonImage = Image(SafirImageBuilder.DefaultCommonImageName);
        AgentImage = Image(SafirImageBuilder.DefaultAgentImageName);
        ManagerImage = Image(SafirImageBuilder.DefaultManagerImageName);
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

    private IDockerImage Image(string prefix) => new DockerImage($"{prefix}:{_tagSuffix}-{SafirImageBuilder.DefaultTag}");
}
