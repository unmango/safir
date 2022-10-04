using System.CommandLine.Binding;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

public static class Bind
{
    public static BinderBase<T> FromServiceProvider<T>()
        where T : class
        => ServiceProviderBinder<T>.Instance;

    internal class ServiceProviderBinder<T> : BinderBase<T>
        where T : class
    {
        public static ServiceProviderBinder<T> Instance { get; } = new();

        protected override T GetBoundValue(BindingContext bindingContext) => bindingContext.GetRequiredService<T>();
    }
}
