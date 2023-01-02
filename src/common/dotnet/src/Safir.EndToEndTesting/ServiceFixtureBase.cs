using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Images;
using JetBrains.Annotations;
using Safir.XUnit.AspNetCore;
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

        Image = new DockerImage(image);

        _dockerFile = dockerFile;
    }

    public IDockerImage BaseImage => SafirImageBuilder.CommonImage;

    public IDockerImage Image { get; }

    public async Task InitializeAsync()
    {
        await SafirImageBuilder.CreateCommon().Build();

        await new ImageFromDockerfileBuilder()
            .WithName(Image)
            .WithDockerfile(_dockerFile)
            .WithDockerfileDirectory(_commonDirectoryPath, "src")
            .WithBuildArgument("CommonImage", BaseImage.FullName)
            .WithDeleteIfExists(false)
            .WithCleanUp(false)
            .Build();
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
