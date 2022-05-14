using System.CommandLine.Binding;

namespace Safir.Cli.DependencyInjection;

public interface IBinderFactory
{
    BinderBase<T> Create<T>() where T : class;
}
