using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;

namespace Safir.Cli.Commands.Add
{
    internal class AddCommand : Command
    {
        public const string NAME = "add";
        public const string DESCRIPTION = "";

        private readonly List<ICommandHandler> _handlers = new List<ICommandHandler>();

        public AddCommand() : base(NAME, DESCRIPTION)
        {
            AddArgument(Path);
        }

        public Argument<FileInfo> Path { get; } = new Argument<FileInfo>("path");

        public void AddHandler(ICommandHandler handler)
        {
            _handlers.Add(handler);
        }

        public static T Register<T>(T builder) where T : CommandLineBuilder
        {
            var command = new AddCommand();

            ArchiveHandler.Register(command);
            DirectoryHandler.Register(command);
            FileHandler.Register(command);

            builder.UseMiddleware(context =>
            {
                if (context.ParseResult.CommandResult.Command is AddCommand)
                {
                    command.Handler = command._handlers.FirstOrDefault();
                }
            });

            return builder.AddCommand(command);
        }
    }
}
