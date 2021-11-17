using Xunit;

namespace Cli.Tests.Helpers
{
    public class NullOrWhitespaceStrings : TheoryData<string?>
    {
        public NullOrWhitespaceStrings()
        {
            Add(null);
            Add("");
            Add(" ");
            Add("\t");
            Add("\n");
        }
    }
}
