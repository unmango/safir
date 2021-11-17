using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cli.Services.Configuration;
using Cli.Services.Configuration.Validation;
using FluentValidation.Results;

namespace Cli.Services.Sources
{
    // TODO: Infer source type?
    internal static class ServiceSourceExtensions
    {
        public static IServiceSource GetSource(this ServiceSource source)
            => source.InferSourceType() switch {
                SourceType.Docker
                    => throw new InvalidOperationException("Unable to create concrete \"Docker\" source type"),
                SourceType.DockerBuild => source.GetDockerBuildSource(),
                SourceType.DockerImage => source.GetDockerImageSource(),
                SourceType.DotnetTool => source.GetDotnetToolSource(),
                SourceType.Git => source.GetGitSource(),
                SourceType.LocalDirectory => source.GetLocalDirectorySource(),
                null => throw new InvalidOperationException("Unable to infer source type"),
                _ => throw new NotSupportedException($"Source type {source.Type} is not supported")
            };

        public static bool TryGetSource(this ServiceSource source, out IServiceSource concrete)
        {
            var validation = source.Validate();

            concrete = validation.IsValid
                ? source.GetSource()
                : source.GetInvalidSource(validation.Errors);

            return validation.IsValid;
        }

        public static DockerBuildSource GetDockerBuildSource(this ServiceSource source)
        {
            source.ValidateDockerBuild(o => o.ThrowOnFailures());
            return new DockerBuildSource(source.Name!, source.BuildContext!, source.Tag);
        }

        public static DockerImageSource GetDockerImageSource(this ServiceSource source)
        {
            source.ValidateDockerImage(o => o.ThrowOnFailures());
            return new DockerImageSource(source.Name!, source.ImageName!, source.Tag);
        }

        public static DotnetToolSource GetDotnetToolSource(this ServiceSource source)
        {
            source.ValidateDotnetTool(o => o.ThrowOnFailures());
            return new DotnetToolSource(source.Name!, source.ToolName!, source.ExtraArgs);
        }

        public static GitSource GetGitSource(this ServiceSource source)
        {
            source.ValidateGit(o => o.ThrowOnFailures());
            return new GitSource(source.Name!, source.CloneUrl!);
        }

        public static LocalDirectorySource GetLocalDirectorySource(this ServiceSource source)
        {
            source.ValidateLocalDirectory(o => o.ThrowOnFailures());
            return new LocalDirectorySource(source.Name!, source.SourceDirectory!);
        }

        public static bool TryGetDockerBuildSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out DockerBuildSource dockerBuild)
            => source.TryGetValidation(
                x => x.ValidateDockerBuild(),
                x => new DockerBuildSource(x.Name!, x.BuildContext!, x.Tag),
                out dockerBuild);

        public static bool TryGetDockerImageSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out DockerImageSource dockerImage)
            => source.TryGetValidation(
                x => x.ValidateDockerImage(),
                x => new DockerImageSource(x.Name!, x.ImageName!, x.Tag),
                out dockerImage);

        public static bool TryGetDotnetToolSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out DotnetToolSource dotnetTool)
            => source.TryGetValidation(
                x => x.ValidateDotnetTool(),
                x => new DotnetToolSource(x.Name!, x.ToolName!, x.ExtraArgs),
                out dotnetTool);

        public static bool TryGetGitSource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out GitSource git)
            => source.TryGetValidation(
                x => x.ValidateGit(),
                x => new GitSource(x.Name!, x.CloneUrl!),
                out git);

        public static bool TryGetLocalDirectorySource(
            this ServiceSource source,
            [MaybeNullWhen(false)] out LocalDirectorySource localDirectory)
            => source.TryGetValidation(
                x => x.ValidateLocalDirectory(),
                x => new LocalDirectorySource(x.Name!, x.SourceDirectory!),
                out localDirectory);

        private static InvalidSource GetInvalidSource(this ServiceSource source, IEnumerable<ValidationFailure> errors)
        {
            return new(source, errors.Select(x => x.ErrorMessage));
        }

        private static bool TryGetValidation<T>(
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
