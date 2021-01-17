using System.Threading;
using System.Threading.Tasks;
using Cli.Services.Sources;

// ReSharper disable NotAccessedField.Local

namespace Cli.Services.Installation.Installers
{
    internal class DotnetToolInstaller : ISourceInstaller<DotnetToolSource>
    {
        private readonly string _toolName;
        private readonly string? _extraArgs;

        public DotnetToolInstaller(string toolName, string? extraArgs)
        {
            _toolName = toolName;
            _extraArgs = extraArgs;
        }

        public ValueTask<ISourceInstalled> GetInstalledAsync(
            SourceContext<DotnetToolSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<IServiceUpdate> GetUpdateAsync(
            SourceContext<DotnetToolSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask InstallAsync(
            SourceContext<DotnetToolSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask UpdateAsync(
            SourceContext<DotnetToolSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
