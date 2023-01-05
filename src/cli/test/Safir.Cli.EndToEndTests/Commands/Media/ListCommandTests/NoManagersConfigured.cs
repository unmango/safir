namespace Safir.Cli.EndToEndTests.Commands.Media.ListCommandTests;

[Trait("Category", "EndToEnd")]
public class NoManagersConfigured : CliTestBase
{
    public NoManagersConfigured(CliFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task List_NoManagersConfigured()
    {
        const string file = "Test.txt";
        await AgentContainer.CreateMediaFileAsync(file);

        var result = await CliBuilder.ExecCliAsync("media", "list");

        if (result.ExitCode > 0)
            Assert.Fail(result.Stderr);

        Assert.Equal("No managers configured\n", result.Stdout);
    }
}
