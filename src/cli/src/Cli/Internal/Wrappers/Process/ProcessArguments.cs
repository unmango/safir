using System.Diagnostics;

namespace Cli.Internal.Wrappers.Process
{
    internal record ProcessArguments(int? Id = null, ProcessStartInfo? StartInfo = null);
}
