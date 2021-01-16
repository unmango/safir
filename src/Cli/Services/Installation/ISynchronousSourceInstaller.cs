using Cli.Internal.Pipeline;

namespace Cli.Services.Installation
{
    internal interface ISynchronousSourceInstaller<in T> : IAppliesTo<T>
        where T : IServiceSource
    {
        IServiceInstalled GetInstalled(InstallationContext context);

        IServiceUpdate GetUpdate(InstallationContext context);

        void Install(InstallationContext context);

        void Update(InstallationContext context);
    }
}
