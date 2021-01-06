using Xunit;

namespace Cli.Tests.Services
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
