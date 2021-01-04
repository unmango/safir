using System.Threading;
using System.Threading.Tasks;

namespace Cli.Services.Installers
{
    internal class DockerBuildInstaller : IServiceInstaller
    {
        private readonly string _buildContext;
        private readonly string? _tag;

        public DockerBuildInstaller(string buildContext, string? tag)
        {
            _buildContext = buildContext;
            _tag = tag;
        }
        
        public ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
