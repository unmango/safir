using System;
using System.CommandLine.Invocation;
using System.Reflection;
using System.Threading.Tasks;

namespace Cli
{
    internal abstract class CommandHandlerBase : ICommandHandler
    {
        private readonly ICommandHandler _inner;

        protected CommandHandlerBase(ICommandHandler inner)
        {
            _inner = inner;
        }
        
        Task<int> ICommandHandler.InvokeAsync(InvocationContext context)
        {
            return _inner.InvokeAsync(context);
        }
        
        protected static ICommandHandler Create(Delegate @delegate) =>
            CommandHandler.Create(@delegate);

        protected static ICommandHandler Create(MethodInfo method, object? target = null) =>
            CommandHandler.Create(method, target);

        protected static ICommandHandler Create(Action action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T>(
            Action<T> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2>(
            Action<T1, T2> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3>(
            Action<T1, T2, T3> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4>(
            Action<T1, T2, T3, T4> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5>(
            Action<T1, T2, T3, T4, T5> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6>(
            Action<T1, T2, T3, T4, T5, T6> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7>(
            Action<T1, T2, T3, T4, T5, T6, T7> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7, T8>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create(Func<int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T>(
            Func<T, int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2>(
            Func<T1, T2, int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3>(
            Func<T1, T2, T3, int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4>(
            Func<T1, T2, T3, T4, int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5>(
            Func<T1, T2, T3, T4, T5, int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6>(
            Func<T1, T2, T3, T4, T5, T6, int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7>(
            Func<T1, T2, T3, T4, T5, T6, T7, int> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create(Func<Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T>(
            Func<T, Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2>(
            Func<T1, T2, Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3>(
            Func<T1, T2, T3, Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4>(
            Func<T1, T2, T3, T4, Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5>(
            Func<T1, T2, T3, T4, T5, Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6>(
            Func<T1, T2, T3, T4, T5, T6, Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7>(
            Func<T1, T2, T3, T4, T5, T6, T7, Task> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create(Func<Task<int>> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T>(
            Func<T, Task<int>> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2>(
            Func<T1, T2, Task<int>> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3>(
            Func<T1, T2, T3, Task<int>> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4>(
            Func<T1, T2, T3, T4, Task<int>> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5>(
            Func<T1, T2, T3, T4, T5, Task<int>> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6>(
            Func<T1, T2, T3, T4, T5, T6, Task<int>> action) =>
            CommandHandler.Create(action);

        protected static ICommandHandler Create<T1, T2, T3, T4, T5, T6, T7>(
            Func<T1, T2, T3, T4, T5, T6, T7, Task<int>> action) =>
            CommandHandler.Create(action);
    }
}
