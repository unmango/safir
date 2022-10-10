using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;

namespace Safir.Manager.EndToEndTests;

public sealed class ManagerServiceFixture : IAsyncLifetime
{
    public IDockerImage Image { get; } = new DockerImage("safir-manager");

    public Task InitializeAsync()
        => new ImageFromDockerfileBuilder()
            .WithName(Image)
            .WithDockerfile("Dockerfile")
            .WithDockerfileDirectory(
                CommonDirectoryPath.GetGitDirectory(),
                Path.Combine("src", "manager"))
            .Build();

    public Task DisposeAsync() => Task.CompletedTask;
}
