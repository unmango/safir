using System;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace Cli.Services.Sources.Validation
{
    internal static class ServiceSourceExtensions
    {
        private static readonly IValidator<ServiceSource> _dockerBuild = new DockerBuildValidator();
        private static readonly IValidator<ServiceSource> _dockerImage = new DockerImageValidator();
        private static readonly IValidator<ServiceSource> _dotnetTool = new DotnetToolValidator();
        private static readonly IValidator<ServiceSource> _git = new GitValidator();
        private static readonly IValidator<ServiceSource> _localDirectory = new LocalDirectoryValidator();

        public static ValidationResult Validate(
            this ServiceSource source,
            Action<ValidationStrategy<ServiceSource>>? options = null)
        {
            if (!source.TryInferSourceType(out var inferred))
                throw new NotSupportedException("Unable to infer source type to validate");
            
            return source.Validate(inferred, options);
        }

        public static ValidationResult Validate(
            this ServiceSource source,
            SourceType type,
            Action<ValidationStrategy<ServiceSource>>? options = null)
            => type switch {
                // TODO: I can do better than throw here
                SourceType.Docker => throw new NotSupportedException("Can't validate \"Docker\" source type"),
                SourceType.DockerBuild => source.ValidateDockerBuild(options),
                SourceType.DockerImage => source.ValidateDockerImage(options),
                SourceType.DotnetTool => source.ValidateDotnetTool(options),
                SourceType.Git => source.ValidateGit(options),
                SourceType.LocalDirectory => source.ValidateLocalDirectory(options),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid source type")
            };

        public static ValidationResult ValidateDockerBuild(
            this ServiceSource source,
            Action<ValidationStrategy<ServiceSource>>? options = null)
            => _dockerBuild.Validate(source, options ?? (_ => { }));

        public static ValidationResult ValidateDockerImage(
            this ServiceSource source,
            Action<ValidationStrategy<ServiceSource>>? options = null)
            => _dockerImage.Validate(source, options ?? (_ => { }));

        public static ValidationResult ValidateDotnetTool(
            this ServiceSource source,
            Action<ValidationStrategy<ServiceSource>>? options = null)
            => _dotnetTool.Validate(source, options ?? (_ => { }));

        public static ValidationResult ValidateGit(
            this ServiceSource source,
            Action<ValidationStrategy<ServiceSource>>? options = null)
            => _git.Validate(source, options ?? (_ => { }));

        public static ValidationResult ValidateLocalDirectory(
            this ServiceSource source,
            Action<ValidationStrategy<ServiceSource>>? options = null)
            => _localDirectory.Validate(source, options ?? (_ => { }));
    }
}
