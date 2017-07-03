using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Safir.Manager.Command.Files
{
    public static partial class Command
    {
        public static void Execute(
            this ICommandHandler<Files.Load> handler) {
            handler.Execute();
        }

        public static partial class Files
        {
            public class Load : ICommand
            {
                internal Load() {

                }

                public event EventHandler CanExecuteChanged;

                public bool CanExecute(object parameter) {
                    throw new NotImplementedException();
                }

                public void Execute(object parameter) {
                    throw new NotImplementedException();
                }
            }

            internal static partial class Handlers
            {
                internal sealed class GetLoadFilesHandler
                    : ICommandHandler<LoadFiles>
                {
                    public void Execute(LoadFiles command) {

                    }
                }
            }
        }
    }
}
