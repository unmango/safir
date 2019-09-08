using System;

namespace Safir.Common.State
{
    public interface IStore<out TState> : IObservable<TState>
    {
        TState Current { get; }

        void Dispatch<TAction>(TAction action) where TAction : IAction;
    }
}
