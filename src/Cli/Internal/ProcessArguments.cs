using System.Diagnostics;

namespace Cli.Internal
{
    internal record ProcessArguments(int? Id = null, ProcessStartInfo? StartInfo = null);
}
