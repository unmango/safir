using System.Threading;
using System.Threading.Tasks;
using Cli.Services.Sources;

// ReSharper disable NotAccessedField.Local

namespace Cli.Services.Installation.Installers
{
    internal class DockerImageInstaller : ISourceInstaller<DockerImageSource>
    {
        private readonly string _imageName;
        private readonly string? _tag;

        public DockerImageInstaller(string imageName, string? tag)
        {
            _imageName = imageName;
            _tag = tag;
        }

        public ValueTask<ISourceInstalled> GetInstalledAsync(
            SourceContext<DockerImageSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask<IServiceUpdate> GetUpdateAsync(
            SourceContext<DockerImageSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask InstallAsync(
            SourceContext<DockerImageSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask UpdateAsync(
            SourceContext<DockerImageSource> context,
            CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
