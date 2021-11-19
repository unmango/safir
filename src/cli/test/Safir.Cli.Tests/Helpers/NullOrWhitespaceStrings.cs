using Xunit;

namespace Safir.Cli.Tests.Helpers
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
