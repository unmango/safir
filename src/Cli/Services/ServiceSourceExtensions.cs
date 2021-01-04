using System;
using System.Collections.Generic;
using System.Linq;
using Cli.Services.Installers;

namespace Cli.Services
{
    internal static class ServiceSourceExtensions
    {
        public static IServiceInstaller GetInstaller(this ServiceSource source)
            => source.InferSourceType() switch {
                SourceType.Docker => GetDockerInstaller(source),
                SourceType.DockerBuild => GetDockerBuildInstaller(source),
                SourceType.DockerImage => GetDockerImageInstaller(source),
                SourceType.DotnetTool => GetDotnetToolInstaller(source),
                SourceType.Git => GetGitInstaller(source),
                SourceType.LocalDirectory => GetLocalDirectoryInstaller(source),
                null => throw new InvalidOperationException("SourceType must be set to retrieve installer"),
                _ => throw new NotSupportedException("SourceType is not supported")
            };

        public static IServiceInstaller GetDockerInstaller(this ServiceSource source)
        {
            while (true)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (source.Type == SourceType.DockerBuild) return GetDockerBuildInstaller(source);
                if (source.Type == SourceType.DockerImage) return GetDockerImageInstaller(source);

                if (source.Type != SourceType.Docker) throw new InvalidOperationException("Invalid SourceType");

                var inferred = InferSourceType(source with { Type = null }, out var updated);

                // Shouldn't ever happen, but will cause an infinite loop if it does.
                // Should hopefully prevent future me from being an idiot.
                if (inferred == SourceType.Docker)
                    throw new InvalidOperationException("Cannot infer \"Docker\" source type");

                source = updated;
            }
        }

        public static IServiceInstaller GetDockerBuildInstaller(this ServiceSource source)
        {
            if (source.Type != SourceType.DockerBuild) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.BuildContext))
                throw new InvalidOperationException("BuildContext must have a value");

            return new DockerBuildInstaller(source.BuildContext, source.Tag);
        }

        public static IServiceInstaller GetDockerImageInstaller(this ServiceSource source)
        {
            if (source.Type != SourceType.DockerImage) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.ImageName))
                throw new InvalidOperationException("ImageName must have a value");

            return new DockerImageInstaller(source.ImageName, source.Tag);
        }

        public static IServiceInstaller GetDotnetToolInstaller(this ServiceSource source)
        {
            if (source.Type != SourceType.DotnetTool) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.ToolName))
                throw new InvalidOperationException("ToolName must have a value");

            return new DotnetToolInstaller(source.ToolName, source.ExtraArgs);
        }

        public static IServiceInstaller GetGitInstaller(this ServiceSource source)
        {
            if (source.Type != SourceType.Git) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.CloneUrl))
                throw new InvalidOperationException("GitCloneUrl must have a value");

            return new GitInstaller(source.CloneUrl);
        }

        public static IServiceInstaller GetLocalDirectoryInstaller(this ServiceSource source)
        {
            if (source.Type != SourceType.LocalDirectory) throw new InvalidOperationException("Invalid SourceType");

            return NoOpInstaller.Value;
        }

        public static IEnumerable<ServiceSource> OrderByPriority(this IEnumerable<ServiceSource> sources)
        {
            return sources.OrderByDescending(x => x.Priority.HasValue).ThenBy(x => x.Priority);
        }

        public static ServiceSource HighestPriority(
            this IEnumerable<ServiceSource> sources,
            Func<ServiceSource, bool>? predicate = null)
        {
            var ordered = sources.OrderByPriority();
            return predicate == null
                ? ordered.First()
                : ordered.First(predicate);
        }

        public static ServiceSource? HighestPriorityOrDefault(
            this IEnumerable<ServiceSource> sources,
            Func<ServiceSource, bool>? predicate = null)
        {
            var ordered = sources.OrderByPriority();
            return predicate == null
                ? ordered.FirstOrDefault()
                : ordered.FirstOrDefault(predicate);
        }

        public static SourceType? InferSourceType(this ServiceSource source)
            => source switch {
                { Type: not null } => source.Type,
                { BuildContext: not null } => SourceType.DockerBuild,
                { ImageName: not null } => SourceType.DockerImage,
                { CloneUrl: not null } => SourceType.Git,
                _ => null,
            };

        public static SourceType? InferSourceType(this ServiceSource source, out ServiceSource updated)
        {
            var inferred = source.InferSourceType();
            updated = source with { Type = inferred };
            return inferred;
        }

        public static bool TryInferSourceType(this ServiceSource source, out SourceType type)
        {
            var inferred = source.InferSourceType();
            type = inferred.GetValueOrDefault();
            return inferred.HasValue;
        }
    }
}
