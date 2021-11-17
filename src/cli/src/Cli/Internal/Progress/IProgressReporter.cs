using System;
using Cli.Internal.Pipeline;

namespace Cli.Internal.Progress
{
    internal interface IProgressReporter : IDisposable
    {
        void Report(ProgressContext context);
    }
}
