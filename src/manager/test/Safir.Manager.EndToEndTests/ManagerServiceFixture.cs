using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Images;
using Xunit.Abstractions;

namespace Safir.Manager.EndToEndTests;

public sealed class ManagerServiceFixture : IAsyncLifetime
{
    public ManagerServiceFixture(IMessageSink sink)
    {
        TestcontainersSettings.Logger = new TestOutputLogger(sink);
    }

    public IDockerImage BaseImage { get; } = new DockerImage("safir-common-dotnet-e2e");

    public IDockerImage Image { get; } = new DockerImage("safir-manager-e2e");

    public async Task InitializeAsync()
    {
        await new ImageFromDockerfileBuilder()
            .WithName(BaseImage)
            .WithDockerfile(Path.Combine("common", "dotnet", "Dockerfile"))
            .WithDockerfileDirectory(CommonDirectoryPath.GetGitDirectory(), "src")
            .Build();

        await new ImageFromDockerfileBuilder()
            .WithName(Image)
            .WithDockerfile("manager/Dockerfile")
            .WithDockerfileDirectory(CommonDirectoryPath.GetGitDirectory(), "src")
            .WithBuildArgument("CommonImage", BaseImage.FullName)
            .Build();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
