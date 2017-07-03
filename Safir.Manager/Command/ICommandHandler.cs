using System.Windows.Input;

namespace Safir.Manager.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
