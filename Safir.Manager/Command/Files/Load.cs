// <copyright file="Load.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Safir.Manager.Command.Files
{
    using System;
    using System.Windows.Input;

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

                public event EventHandler CanExecuteChanged {
                    add { }
                    remove { }
                }

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
