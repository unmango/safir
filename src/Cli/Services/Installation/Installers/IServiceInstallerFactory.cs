namespace Cli.Services.Installation.Installers
{
    internal interface IServiceInstallerFactory
    {
        IServiceInstaller GetDockerBuildInstaller(ServiceSource source);

        IServiceInstaller GetDockerImageInstaller(ServiceSource source);

        IServiceInstaller GetDotnetToolInstaller(ServiceSource source);

        IServiceInstaller GetGitInstaller(ServiceSource source);

        IServiceInstaller GetLocalDirectoryInstaller(ServiceSource source);
    }
}
