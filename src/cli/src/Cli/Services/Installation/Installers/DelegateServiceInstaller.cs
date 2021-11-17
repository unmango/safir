using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installation.Installers
{
    internal class DelegateServiceInstaller : IServiceInstaller
    {
        private readonly InstallAsync _installAsync;

        public DelegateServiceInstaller(InstallAsync installAsync)
        {
            _installAsync = installAsync ?? throw new ArgumentNullException(nameof(installAsync));
        }
        
        public ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
            => _installAsync(context, cancellationToken);
    }
}
