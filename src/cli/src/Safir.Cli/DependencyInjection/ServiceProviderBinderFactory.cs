using System;
using System.CommandLine.Binding;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.Cli.DependencyInjection;

internal class ServiceProviderBinderFactory : IBinderFactory
{
    private readonly Lazy<IServiceProvider> _services;

    public ServiceProviderBinderFactory(IServiceCollection services)
    {
        _services = new(services.BuildServiceProvider);
    }

    public BinderBase<T> Create<T>() where T : class => new ServiceProviderBinder<T>(_services);

    private class ServiceProviderBinder<T> : BinderBase<T>
        where T : class
    {
        private readonly Lazy<IServiceProvider> _services;

        public ServiceProviderBinder(Lazy<IServiceProvider> services)
        {
            _services = services;
        }

        protected override T GetBoundValue(BindingContext bindingContext)
        {
            return _services.Value.GetRequiredService<T>();
        }
    }
}
