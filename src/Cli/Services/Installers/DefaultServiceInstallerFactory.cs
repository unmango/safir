using System;

namespace Cli.Services.Installers
{
    internal class DefaultServiceInstallerFactory : IServiceInstallerFactory
    {
        public IServiceInstaller GetDockerBuildInstaller(ServiceSource source)
        {
            if (source.Type != SourceType.DockerBuild) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.BuildContext))
                throw new InvalidOperationException("BuildContext must have a value");

            return new DockerBuildInstaller(source.BuildContext, source.Tag);
        }

        public IServiceInstaller GetDockerImageInstaller(ServiceSource source)
        {
            if (source.Type != SourceType.DockerImage) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.ImageName))
                throw new InvalidOperationException("ImageName must have a value");

            return new DockerImageInstaller(source.ImageName, source.Tag);
        }

        public IServiceInstaller GetDotnetToolInstaller(ServiceSource source)
        {
            if (source.Type != SourceType.DotnetTool) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.ToolName))
                throw new InvalidOperationException("ToolName must have a value");

            return new DotnetToolInstaller(source.ToolName, source.ExtraArgs);
        }

        public IServiceInstaller GetGitInstaller(ServiceSource source)
        {
            if (source.Type != SourceType.Git) throw new InvalidOperationException("Invalid SourceType");
            if (string.IsNullOrWhiteSpace(source.CloneUrl))
                throw new InvalidOperationException("GitCloneUrl must have a value");

            return new GitInstaller(source.CloneUrl);
        }

        public IServiceInstaller GetLocalDirectoryInstaller(ServiceSource source)
        {
            if (source.Type != SourceType.LocalDirectory) throw new InvalidOperationException("Invalid SourceType");

            return NoOpInstaller.Value;
        }
    }
}
