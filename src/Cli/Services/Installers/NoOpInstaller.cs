using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installers
{
    internal sealed class NoOpInstaller : IServiceInstaller
    {
        public static readonly NoOpInstaller Value = new();

        private NoOpInstaller() { }
        
        public ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            return new ValueTask();
        }
    }
}
