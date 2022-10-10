using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace Safir.Manager.EndToEndTests;

public sealed class ManagerServiceFixture : IAsyncLifetime
{
    public IDockerImage Image { get; } = new DockerImage("safir-manager-e2e");

    public Task InitializeAsync()
        => new ImageFromDockerfileBuilder()
            .WithName(Image)
            .WithDockerfile("manager/Dockerfile")
            .WithDockerfileDirectory(CommonDirectoryPath.GetGitDirectory(), "src")
            .Build();

    public Task DisposeAsync() => Task.CompletedTask;
}
