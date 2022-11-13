using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Images;
using JetBrains.Annotations;
using Xunit;
using Xunit.Abstractions;

namespace Safir.EndToEndTesting;

[PublicAPI]
public class ServiceFixtureBase : IAsyncLifetime
{
    private readonly CommonDirectoryPath _commonDirectoryPath = CommonDirectoryPath.GetGitDirectory();
    private readonly string _dockerFile;

    public ServiceFixtureBase(IMessageSink sink, string image, string dockerFile, string baseImage = "safir-common-dotnet:e2e")
    {
        TestcontainersSettings.Logger = new MessageSinkLogger(sink);

        BaseImage = new DockerImage(baseImage);
        Image = new DockerImage(image);

        _dockerFile = dockerFile;
    }

    public IDockerImage BaseImage { get; }

    public IDockerImage Image { get; }

    public async Task InitializeAsync()
    {
        await new ImageFromDockerfileBuilder()
            .WithName(BaseImage)
            .WithDockerfile(Path.Combine("common", "dotnet", "Dockerfile"))
            .WithDockerfileDirectory(_commonDirectoryPath, "src")
            .Build();

        await new ImageFromDockerfileBuilder()
            .WithName(Image)
            .WithDockerfile(_dockerFile)
            .WithDockerfileDirectory(_commonDirectoryPath, "src")
            .WithBuildArgument("CommonImage", BaseImage.FullName)
            .Build();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
