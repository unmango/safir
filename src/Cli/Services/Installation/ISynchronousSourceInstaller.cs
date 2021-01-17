using Cli.Internal.Pipeline;

namespace Cli.Services.Installation
{
    internal interface ISynchronousSourceInstaller<in T> : IAppliesTo<T>
        where T : IServiceSource
    {
        ISourceInstalled GetInstalled(T source, InstallationContext context);

        IServiceUpdate GetUpdate(T source, InstallationContext context);

        void Install(T source, InstallationContext context);

        void Update(T source, InstallationContext context);
    }
}
