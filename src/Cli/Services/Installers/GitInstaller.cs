using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cli.Services.Sources;

// ReSharper disable NotAccessedField.Local

namespace Cli.Services.Installers
{
    internal class GitInstaller : PipelineServiceInstaller
    {
        private readonly string? _cloneUrl;

        public GitInstaller() { }
        
        public GitInstaller(string cloneUrl)
        {
            _cloneUrl = cloneUrl;
        }

        public override bool AppliesTo(InstallationContext context)
        {
            return context.Sources.Any(x => x.TryGetGitSource(out _));
        }

        public override ValueTask InstallAsync(InstallationContext context, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
