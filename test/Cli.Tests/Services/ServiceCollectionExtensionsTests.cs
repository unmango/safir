using System.Collections.Generic;
using System.Linq;
using Cli.Services;
using Cli.Services.Installers;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cli.Tests.Services
{
    public class ServiceCollectionExtensionsTests
    {
        private readonly ServiceCollection _services = new();

        [Fact]
        public void CanResolveInstallationPipeline()
        {
            var services = _services.AddServiceInstallationPipeline()
                .BuildServiceProvider();

            var pipeline = services.GetService<IInstallationPipeline>();
            
            Assert.NotNull(pipeline);
        }

        [Fact]
        public void CanResolvePipelineServiceInstallers()
        {
            var services = _services.AddServiceInstallationPipeline()
                .BuildServiceProvider();

            var installers = services.GetService<IEnumerable<IPipelineServiceInstaller>>();
            
            Assert.NotNull(installers);
            Assert.True(installers!.Any());
        }
    }
}
