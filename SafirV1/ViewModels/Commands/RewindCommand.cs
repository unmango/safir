using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Safir.ViewModels.Commands
{
    internal class RewindCommand : ICommand
    {
        public bool CanExecute(object parameter)
        {
            // TODO: Code
            return false;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
