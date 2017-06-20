namespace Safir.Manager.Command
{
    public interface ICommandHandler<TCommand>
    {
        void Execute(TCommand command);
    }
}
