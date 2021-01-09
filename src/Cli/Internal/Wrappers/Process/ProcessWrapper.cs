using System.Diagnostics;

namespace Cli.Internal.Wrappers.Process
{
    /// <summary>
    /// Wraps <see cref="Process"/>.
    /// </summary>
    internal sealed class ProcessWrapper : IProcess
    {
        public ProcessWrapper(System.Diagnostics.Process? process = null)
        {
            Process = process ?? new System.Diagnostics.Process();
        }

        public int Id => Process.Id;
        
        public System.Diagnostics.Process Process { get; }

        public ProcessStartInfo StartInfo => Process.StartInfo;

        public bool Start() => Process.Start();

        public void Dispose() => Process.Dispose();
    }
}
