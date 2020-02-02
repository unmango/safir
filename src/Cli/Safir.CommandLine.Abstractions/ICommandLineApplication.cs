using System.Threading;
using System.Threading.Tasks;

namespace System.CommandLine
{
    public interface ICommandLineApplication : IDisposable
    {
        IServiceProvider Services { get; }

        Task<int> RunAsync(string[] args, CancellationToken cancellationToken = default);
    }
}
