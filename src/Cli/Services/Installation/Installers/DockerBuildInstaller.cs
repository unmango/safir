using System.Threading;
using System.Threading.Tasks;
using Cli.Services.Sources;

// ReSharper disable NotAccessedField.Local

namespace Cli.Services.Installation.Installers
{
    internal class DockerBuildInstaller : ISourceInstaller<DockerBuildSource>
    {
        private readonly string _buildContext;
        private readonly string? _tag;

        public DockerBuildInstaller(string buildContext, string? tag)
        {
            _buildContext = buildContext;
            _tag = tag;
        }

        public ValueTask<ISourceInstalled> GetInstalledAsync(
            SourceContext<DockerBuildSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<IServiceUpdate> GetUpdateAsync(
            SourceContext<DockerBuildSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask InstallAsync(
            SourceContext<DockerBuildSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask UpdateAsync(
            SourceContext<DockerBuildSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
