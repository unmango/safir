using System;
using System.Diagnostics.CodeAnalysis;
using Cli.Services.Sources.Validation;
using FluentValidation.Results;

namespace Cli.Services.Sources
{
    // TODO: Infer source type?
    internal static class ServiceSourceExtensions
    {
        public static DockerBuildSource GetDockerBuildSource(this ServiceSource source)
        {
            source.ValidateDockerBuild(o => o.ThrowOnFailures());
            return new DockerBuildSource(source.BuildContext!, source.Tag);
        }

        public static DockerImageSource GetDockerImageSource(this ServiceSource source)
        {
            source.ValidateDockerImage(o => o.ThrowOnFailures());
            return new DockerImageSource(source.ImageName!, source.Tag);
        }

        public static DotnetToolSource GetDotnetToolSource(this ServiceSource source)
        {
            source.ValidateDotnetTool(o => o.ThrowOnFailures());
            return new DotnetToolSource(source.ToolName!, source.ExtraArgs);
        }

        public static GitSource GetGitSource(this ServiceSource source)
        {
            source.ValidateGit(o => o.ThrowOnFailures());
            return new GitSource(source.CloneUrl!);
        }

        public static LocalDirectorySource GetLocalDirectorySource(this ServiceSource source)
        {
            source.ValidateLocalDirectory(o => o.ThrowOnFailures());
            return new LocalDirectorySource(source.SourceDirectory!);
        }

        public static bool TryGetDockerBuildSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out DockerBuildSource dockerBuild)
            => source.TryGet(
                x => x.ValidateDockerBuild(),
                x => new DockerBuildSource(x.BuildContext!, x.Tag),
                out dockerBuild);

        public static bool TryGetDockerImageSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out DockerImageSource dockerImage)
            => source.TryGet(
                x => x.ValidateDockerImage(),
                x => new DockerImageSource(x.ImageName!, x.Tag),
                out dockerImage);

        public static bool TryGetDotnetToolSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out DotnetToolSource dotnetTool)
            => source.TryGet(
                x => x.ValidateDotnetTool(),
                x => new DotnetToolSource(x.ToolName!, x.ExtraArgs),
                out dotnetTool);

        public static bool TryGetGitSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out GitSource git)
            => source.TryGet(
                x => x.ValidateGit(),
                x => new GitSource(x.CloneUrl!),
                out git);

        public static bool TryGetLocalDirectorySource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out LocalDirectorySource localDirectory)
            => source.TryGet(
                x => x.ValidateLocalDirectory(),
                x => new LocalDirectorySource(x.SourceDirectory!),
                out localDirectory);

        private static bool TryGet<T>(
            this ServiceSource source,
            Func<ServiceSource, ValidationResult> validator,
            Func<ServiceSource, T> factory,
            [MaybeNullWhen(false)] out T gotten)
        {
            gotten = default;
            var result = validator(source);
            if (result.IsValid) gotten = factory(source);
            return result.IsValid;
        }
    }
}
