using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;

namespace Cli.Internal
{
    internal static class ConsoleText
    {
        public static View Underline(string text)
            => new ContentView(new ContainerSpan(
                StyleSpan.UnderlinedOn(),
                new ContentSpan(text),
                StyleSpan.UnderlinedOff()));
    }
}
