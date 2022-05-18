using Safir.Cli.Configuration;
using Xunit;

namespace Safir.Cli.Tests.Configuration;

public class SafirOptionsTests
{
    // TODO: Probably fails on Windows
    [Fact]
    public void File_ConcatenatesConfigFileNameToConfigDirectory()
    {
        const string directory = "/my/test/directory";
        const string expectedFile = $"{directory}/config.json";
        var options = new SafirOptions {
            Config = new() {
                Directory = directory,
            },
        };

        var result = options.Config.File;

        Assert.Equal(expectedFile, result);
    }
}
