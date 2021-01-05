using System;

namespace Cli.Services
{
    internal static class ServiceInstallerFactoryExtensions
    {
        public static IServiceInstaller GetInstaller(this IServiceInstallerFactory factory, ServiceSource source)
            => source.InferSourceType() switch {
                SourceType.Docker => factory.GetDockerInstaller(source),
                SourceType.DockerBuild => factory.GetDockerBuildInstaller(source),
                SourceType.DockerImage => factory.GetDockerImageInstaller(source),
                SourceType.DotnetTool => factory.GetDotnetToolInstaller(source),
                SourceType.Git => factory.GetGitInstaller(source),
                SourceType.LocalDirectory => factory.GetLocalDirectoryInstaller(source),
                null => throw new InvalidOperationException("SourceType must be set to retrieve installer"),
                _ => throw new NotSupportedException("SourceType is not supported")
            };
        
        public static IServiceInstaller GetDockerInstaller(this IServiceInstallerFactory factory, ServiceSource source)
        {
            while (true)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (source.Type == SourceType.DockerBuild) return factory.GetDockerBuildInstaller(source);
                if (source.Type == SourceType.DockerImage) return factory.GetDockerImageInstaller(source);

                if (source.Type != SourceType.Docker) throw new InvalidOperationException("Invalid SourceType");

                var inferred = (source with { Type = null }).InferSourceType(out var updated);

                // Shouldn't ever happen, but will cause an infinite loop if it does.
                // Should hopefully prevent future me from being an idiot.
                if (inferred == SourceType.Docker)
                    throw new InvalidOperationException("Cannot infer \"Docker\" source type");

                source = updated;
            }
        }
    }
}
