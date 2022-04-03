using System;

namespace Safir.Cli.Internal.Progress;

internal interface IProgressReporter : IDisposable
{
    void Report(ProgressContext context);
}