using System.Threading;
using System.Threading.Tasks;
// ReSharper disable NotAccessedField.Local

namespace Cli.Services.Installers
{
    internal class DotnetToolInstaller : IServiceInstaller
    {
        private readonly string _toolName;
        private readonly string? _extraArgs;

        public DotnetToolInstaller(string toolName, string? extraArgs)
        {
            _toolName = toolName;
            _extraArgs = extraArgs;
        }
        
        public ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
