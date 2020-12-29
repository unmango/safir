using System.Diagnostics;

namespace Cli.Internal
{
    /// <summary>
    /// Wraps <see cref="Process"/>.
    /// </summary>
    internal sealed class ProcessWrapper : IProcess
    {
        public ProcessWrapper(Process? process = null)
        {
            Process = process ?? new Process();
        }

        public int Id => Process.Id;
        
        public Process Process { get; }

        public ProcessStartInfo StartInfo => Process.StartInfo;

        public bool Start() => Process.Start();

        public void Dispose() => Process.Dispose();
    }
}
