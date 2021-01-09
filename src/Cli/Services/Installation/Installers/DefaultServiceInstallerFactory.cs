using System;
using Cli.Internal.Wrappers.Git;
using Cli.Services.Sources;
using Cli.Services.Sources.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Services.Installation.Installers
{
    internal class DefaultServiceInstallerFactory : IServiceInstallerFactory
    {
        private readonly IServiceProvider _services;

        public DefaultServiceInstallerFactory(IServiceProvider services)
        {
            _services = services;
        }
        
        public IServiceInstaller GetDockerBuildInstaller(ServiceSource source)
        {
            var (buildContext, tag) = source.GetDockerBuildSource();
            return new DockerBuildInstaller(buildContext, tag);
        }

        public IServiceInstaller GetDockerImageInstaller(ServiceSource source)
        {
            var (imageName, tag) = source.GetDockerImageSource();
            return new DockerImageInstaller(imageName, tag);
        }

        public IServiceInstaller GetDotnetToolInstaller(ServiceSource source)
        {
            var (toolName, extraArgs) = source.GetDotnetToolSource();
            return new DotnetToolInstaller(toolName, extraArgs);
        }

        public IServiceInstaller GetGitInstaller(ServiceSource source)
        {
            var gitSource = source.GetGitSource();
            var repository = _services.GetRequiredService<IRepositoryFunctions>();
            return new GitInstaller(gitSource.CloneUrl, repository);
        }

        public IServiceInstaller GetLocalDirectoryInstaller(ServiceSource source)
        {
            _ = source.ValidateLocalDirectory(options => options
                .IncludeProperties(x => x.Type)
                .ThrowOnFailures());
            
            return NoOpInstaller.Value;
        }
    }
}
