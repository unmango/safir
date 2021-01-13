using System.Threading;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    public interface IPipeline<in T>
        where T : class
    {
        ValueTask InvokeAsync(T context, CancellationToken cancellationToken);
    }
}
