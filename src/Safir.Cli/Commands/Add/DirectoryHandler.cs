using System;
using System.CommandLine;
using System.IO;
using System.Threading.Tasks;

namespace Safir.Cli.Commands.Add
{
    internal class DirectoryHandler : CommandHandlerBase<FileInfo>
    {
        public static void Register(AddCommand command)
        {
            command.AddHandler(new DirectoryHandler());
        }

        protected override Task Execute(FileInfo arg)
        {
            throw new NotImplementedException();
        }
    }
}
