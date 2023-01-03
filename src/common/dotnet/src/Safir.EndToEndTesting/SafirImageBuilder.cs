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
    public const string DefaultCliImageName = "safir-cli";

    public static readonly IDockerImage CommonImage = new DockerImage($"{DefaultCommonImageName}:{DefaultTag}");

    public static readonly IDockerImage AgentImage = new DockerImage($"{DefaultAgentImageName}:{DefaultTag}");

    public static readonly IDockerImage ManagerImage = new DockerImage($"{DefaultManagerImageName}:{DefaultTag}");

    public static readonly IDockerImage CliImage = new DockerImage($"{DefaultCliImageName}:{DefaultTag}");

    public static IImageFromDockerfileBuilder Create()
        => new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetGitDirectory(), "src")
            .WithDeleteIfExists(false)
            .WithCleanUp(false);

    public static IImageFromDockerfileBuilder WithSafirCommonBaseImage(
        this IImageFromDockerfileBuilder builder,
        IDockerImage? baseImage = null)
        => builder.WithBuildArgument(CommonImageBuildArg, (baseImage ?? CommonImage).FullName);

    public static IImageFromDockerfileBuilder WithSafirCommonDockerfile(this IImageFromDockerfileBuilder builder)
        => builder.WithDockerfile(Path.Combine("common", "dotnet", "Dockerfile"));

    public static IImageFromDockerfileBuilder WithSafirAgentDockerfile(this IImageFromDockerfileBuilder builder)
        => builder.WithDockerfile(Path.Combine("agent", "Dockerfile"));

    public static IImageFromDockerfileBuilder WithSafirManagerDockerfile(this IImageFromDockerfileBuilder builder)
        => builder.WithDockerfile(Path.Combine("manager", "Dockerfile"));

    public static IImageFromDockerfileBuilder WithSafirCliDockerfile(this IImageFromDockerfileBuilder builder)
        => builder.WithDockerfile(Path.Combine("cli", "Dockerfile"));

    public static IImageFromDockerfileBuilder WithSafirCommonConfiguration(this IImageFromDockerfileBuilder builder)
        => builder.WithSafirCommonDockerfile()
            .WithName(CommonImage);

    public static IImageFromDockerfileBuilder WithSafirAgentConfiguration(
        this IImageFromDockerfileBuilder builder,
        IDockerImage? baseImage = null)
        => builder.WithSafirAgentDockerfile()
            .WithSafirCommonBaseImage(baseImage)
            .WithName(AgentImage);

    public static IImageFromDockerfileBuilder WithSafirManagerConfiguration(
        this IImageFromDockerfileBuilder builder,
        IDockerImage? baseImage = null)
        => builder.WithSafirManagerDockerfile()
            .WithSafirCommonBaseImage(baseImage)
            .WithName(ManagerImage);

    public static IImageFromDockerfileBuilder WithSafirCliConfiguration(this IImageFromDockerfileBuilder builder)
        => builder.WithSafirCliDockerfile()
            .WithName(CliImage);
}
