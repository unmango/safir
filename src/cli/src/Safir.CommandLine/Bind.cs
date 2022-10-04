using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using Microsoft.Extensions.DependencyInjection;

namespace Safir.CommandLine;

public static class Bind
{
    private const string NoHandlerContextMessage =
        $"Unable to get handler context. Has {nameof(CommandLineBuilderExtensions.UseCommandHandlers)} been called?";

    public static BinderBase<CancellationToken> CancellationToken() => CancellationTokenBinder.Instance;

    public static BinderBase<T> FromHandlerContext<T>()
        where T : class
        => HandlerContextBinder<T>.Instance;

    public static BinderBase<T> FromServiceProvider<T>()
        where T : class
        => ServiceProviderBinder<T>.Instance;

    internal sealed class CancellationTokenBinder : BinderBase<CancellationToken>
    {
        public static CancellationTokenBinder Instance { get; } = new();

        protected override CancellationToken GetBoundValue(BindingContext bindingContext)
        {
            var context = bindingContext.GetRequiredService<InvocationContext>();
            return context.GetCancellationToken();
        }
    }

    internal sealed class HandlerContextBinder<T> : BinderBase<T>
        where T : class
    {
        public static HandlerContextBinder<T> Instance { get; } = new();

        protected override T GetBoundValue(BindingContext bindingContext)
        {
            var context = bindingContext.GetService<HandlerContext>();

            if (context is null)
                throw new InvalidOperationException(NoHandlerContextMessage);

            return context.Services.GetRequiredService<T>();
        }
    }

    internal sealed class ServiceProviderBinder<T> : BinderBase<T>
        where T : class
    {
        public static ServiceProviderBinder<T> Instance { get; } = new();

        protected override T GetBoundValue(BindingContext bindingContext) => bindingContext.GetRequiredService<T>();
    }
}
