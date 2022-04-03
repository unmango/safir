using System;
using Microsoft.Extensions.DependencyInjection;
using Safir.Cli.Internal.Progress;
using Safir.Cli.Internal.Wrappers.Git;
using Safir.Cli.Services.Configuration;
using Safir.Cli.Services.Configuration.Validation;
using Safir.Cli.Services.Sources;

namespace Safir.Cli.Services.Installation.Installers;

internal class DefaultServiceInstallerFactory : IServiceInstallerFactory
{
    private readonly IServiceProvider _services;

    public DefaultServiceInstallerFactory(IServiceProvider services)
    {
        _services = services;
    }
        
    public IServiceInstaller GetDockerBuildInstaller(ServiceSource source)
    {
        var (_, buildContext, tag) = source.GetDockerBuildSource();
        return new DockerBuildInstaller(buildContext, tag);
    }

    public IServiceInstaller GetDockerImageInstaller(ServiceSource source)
    {
        var (_, imageName, tag) = source.GetDockerImageSource();
        return new DockerImageInstaller(imageName, tag);
    }

    public IServiceInstaller GetDotnetToolInstaller(ServiceSource source)
    {
        var (_, toolName, extraArgs) = source.GetDotnetToolSource();
        return new DotnetToolInstaller(toolName, extraArgs);
    }

    public IServiceInstaller GetGitInstaller(ServiceSource source)
    {
        var gitSource = source.GetGitSource();
        var repository = _services.GetRequiredService<IRepositoryFunctions>();
        var progress = _services.GetRequiredService<IProgressReporter>();
        return new GitInstaller(gitSource.CloneUrl, repository, progress);
    }

    public IServiceInstaller GetLocalDirectoryInstaller(ServiceSource source)
    {
        _ = source.ValidateLocalDirectory(options => options
            .IncludeProperties(x => x.Type)
            .ThrowOnFailures());
            
        return NoOpInstaller.Value;
    }
}