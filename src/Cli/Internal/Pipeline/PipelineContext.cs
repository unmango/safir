using System;
using System.Threading.Tasks;

namespace Cli.Internal.Pipeline
{
    internal record PipelineContext<T>(Func<T, ValueTask> Next)
        where T : class
    {
    }
}
