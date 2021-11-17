using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Cli.Middleware;
using Moq;
using Xunit;

namespace Cli.Tests.Middleware
{
    public class HelpMiddlewareTests
    {
        private readonly Mock<IHelpBuilder> _help = new();

        public static readonly IEnumerable<object[]> SymbolTestData = new[] {
            new object[] { new Argument("test") },
            new object[] { new Option("-o") },
        };

        [Theory]
        [MemberData(nameof(SymbolTestData))]
        public async Task PrintsHelpWhenSymbolAndNoArguments(Symbol symbol)
        {
            var flag = false;
            var root = new RootCommand {
                Handler = CommandHandler.Create(() => flag = true)
            };
            root.Add(symbol);
            var command = new CommandLineBuilder(root)
                .UseHelpBuilder(_ => _help.Object)
                .UseHelpForEmptyCommands()
                .Build();

            await command.InvokeAsync(string.Empty);

            Assert.False(flag);
            _help.Verify(x => x.Write(root));
        }

        public static readonly IEnumerable<object[]> SymbolWithCommandLineTestData = new[] {
            new object[] { new Argument("test"), "test" },
            new object[] { new Option("-o"), "-o" },
        };

        [Theory]
        [MemberData(nameof(SymbolWithCommandLineTestData))]
        public async Task InvokesCommandWhenSymbolSupplied(Symbol symbol, string commandLine)
        {
            var flag = false;
            var root = new RootCommand {
                Handler = CommandHandler.Create(() => flag = true),
            };
            root.Add(symbol);
            var command = new CommandLineBuilder(root)
                .UseHelpBuilder(_ => _help.Object)
                .UseHelpForEmptyCommands()
                .Build();

            await command.InvokeAsync(commandLine);

            Assert.True(flag);
            _help.Verify(x => x.Write(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task InvokesCommandWhenNoSymbolAndNoCommandLine()
        {
            var flag = false;
            var root = new RootCommand {
                Handler = CommandHandler.Create(() => flag = true),
            };
            var command = new CommandLineBuilder(root)
                .UseHelpBuilder(_ => _help.Object)
                .UseHelpForEmptyCommands()
                .Build();

            await command.InvokeAsync(string.Empty);

            Assert.True(flag);
            _help.Verify(x => x.Write(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task InvokesCommandWhenNoSymbolAndNoCommandLineWithGlobalOptions()
        {
            var flag = false;
            var root = new RootCommand {
                Handler = CommandHandler.Create(() => flag = true),
            };
            var command = new CommandLineBuilder(root)
                .AddGlobalOption(new Option("--global"))
                .UseHelpBuilder(_ => _help.Object)
                .UseHelpForEmptyCommands()
                .Build();

            await command.InvokeAsync(string.Empty);

            Assert.True(flag);
            _help.Verify(x => x.Write(It.IsAny<ICommand>()), Times.Never);
        }

        [Fact]
        public async Task InvokesSubCommandWhenNoSymbolAndNoCommandLineWithGlobalOptions()
        {
            var flag = false;
            var root = new RootCommand();
            root.AddCommand(new Command("sub") {
                Handler = CommandHandler.Create(() => flag = true),
            });
            var command = new CommandLineBuilder(root)
                .AddGlobalOption(new Option("--global"))
                .UseHelpBuilder(_ => _help.Object)
                .UseHelpForEmptyCommands()
                .Build();

            await command.InvokeAsync("sub");

            Assert.True(flag);
            _help.Verify(x => x.Write(It.IsAny<ICommand>()), Times.Never);
        }
    }
}
