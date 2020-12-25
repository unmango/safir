using Cli.Commands.Service;
using System.Linq;
using Xunit;

namespace Cli.Tests.Commands.Service
{
    public class StopCommandTests
    {
        [Fact]
        public void RequiresServiceOption()
        {
            var command = new RestartCommand();

            var options = command.Options.OfType<ServiceOption>();

            var option = Assert.Single(options);
            Assert.NotNull(option);
            Assert.True(option!.IsRequired);
        }
    }
}
