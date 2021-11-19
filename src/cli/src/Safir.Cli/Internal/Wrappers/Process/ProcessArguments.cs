using System.Diagnostics;

namespace Safir.Cli.Internal.Wrappers.Process
{
    internal record ProcessArguments(int? Id = null, ProcessStartInfo? StartInfo = null);
}
