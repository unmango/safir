using Safir.Common.State;
using Safir.FileManager.Store;
using System;

namespace Safir.FileManager.Infrastructure
{
    public class Store : IStore<State>
    {
        public Store(State initialState, Func<IAction, State, State> rootReducer)
        {
            Current = initialState;
        }

        public State Current { get; }

        public void Dispatch<TAction>(TAction action) where TAction : IAction
        {
            throw new NotImplementedException();
        }

        public IDisposable Subscribe(IObserver<State> observer)
        {
            throw new NotImplementedException();
        }
    }
}
