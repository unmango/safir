namespace Cli.Internal.Wrappers.Process
{
    internal class DefaultProcessFactory : IProcessFactory
    {
        public IProcess Create(ProcessArguments? args = null)
        {
            if (args == null) return new ProcessWrapper();
            
            System.Diagnostics.Process process = new();

            if (args.Id.HasValue)
            {
                process = System.Diagnostics.Process.GetProcessById(args.Id.Value);
            }

            if (args.StartInfo != null)
            {
                process.StartInfo = args.StartInfo;
            }
            
            return new ProcessWrapper(process);
        }
    }
}
