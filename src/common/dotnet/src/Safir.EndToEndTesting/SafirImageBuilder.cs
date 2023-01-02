using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Images;
using JetBrains.Annotations;

namespace Safir.EndToEndTesting;

[PublicAPI]
public static class SafirImageBuilder
{
    public const string CommonImageBuildArg = "CommonImage";
    public const string DefaultTag = "e2e";
    public const string DefaultCommonImageName = "safir-common-dotnet";
    public const string DefaultAgentImageName = "safir-agent";
    public const string DefaultManagerImageName = "safir-manager";

    private static readonly CommonDirectoryPath _gitDirectory = CommonDirectoryPath.GetGitDirectory();

    public static readonly IDockerImage CommonImage = new DockerImage($"{DefaultCommonImageName}:{DefaultTag}");

    public static readonly IDockerImage AgentImage = new DockerImage($"{DefaultAgentImageName}:{DefaultTag}");

    public static readonly IDockerImage ManagerImage = new DockerImage($"{DefaultManagerImageName}:{DefaultTag}");

    public static IImageFromDockerfileBuilder CreateCommon()
        => Create(CommonImage, Path.Combine("common", "dotnet", "Dockerfile"));

    public static IImageFromDockerfileBuilder CreateAgent(IDockerImage baseImage)
        => Create(AgentImage, Path.Combine("agent", "Dockerfile"))
            .WithBuildArgument(CommonImageBuildArg, baseImage.FullName);

    public static IImageFromDockerfileBuilder CreateAgent() => CreateAgent(CommonImage);

    public static IImageFromDockerfileBuilder CreateManager(IDockerImage baseImage)
        => Create(ManagerImage, Path.Combine("manager", "Dockerfile"))
            .WithBuildArgument(CommonImageBuildArg, baseImage.FullName);

    public static IImageFromDockerfileBuilder CreateManager() => CreateManager(CommonImage);

    private static IImageFromDockerfileBuilder Create(IDockerImage image, string dockerfile)
        => new ImageFromDockerfileBuilder()
            .WithName(image)
            .WithDockerfile(dockerfile)
            .WithDockerfileDirectory(_gitDirectory, "src")
            .WithDeleteIfExists(false)
            .WithCleanUp(false);
}
