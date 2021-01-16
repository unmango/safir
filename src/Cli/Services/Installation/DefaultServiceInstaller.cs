using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation
{
    internal class DefaultServiceInstaller<T> : IServiceInstaller<T>
        where T : IService
    {
        public bool AppliesTo(InstallationContext context)
        {
            throw new System.NotImplementedException();
        }
        
        public ValueTask InvokeAsync(InstallationContext context, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<bool> IsInstalledAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
