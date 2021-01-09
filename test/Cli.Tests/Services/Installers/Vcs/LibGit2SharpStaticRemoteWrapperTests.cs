using Cli.Services.Installers.Vcs;
using Xunit;

namespace Cli.Tests.Services.Installers.Vcs
{
    // No need to test external libraries, but I want to verify I know how the library works myself.
    public class LibGit2SharpStaticRemoteWrapperTests
    {
        private readonly LibGit2SharpStaticRemoteWrapper _wrapper = new();

        [Theory]
        // [InlineData("http://github.com/unmango/safir-cli.git")]
        [InlineData("test")]
        public void IsValidName(string name)
        {
            var result = _wrapper.IsValidName(name);
            
            Assert.True(result);
        }
    }
}
