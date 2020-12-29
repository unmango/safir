using System.Diagnostics;
using Cli.Services;

namespace Cli.Internal
{
    internal class DefaultProcessFactory : IProcessFactory
    {
        public IProcess Create(ProcessArguments? args = null)
        {
            if (args == null) return new ProcessWrapper();
            
            Process process = new();

            if (args.Id.HasValue)
            {
                process = Process.GetProcessById(args.Id.Value);
            }

            if (args.StartInfo != null)
            {
                process.StartInfo = args.StartInfo;
            }
            
            return new ProcessWrapper(process);
        }
    }
}
