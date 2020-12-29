using Cli.Services;

namespace Cli.Internal
{
    /// <summary>
    /// An abstraction for creating new <see cref="System.Diagnostics.Process"/>es.
    /// </summary>
    internal interface IProcessFactory
    {
        /// <summary>
        /// Creates a new <see cref="IProcess"/> with the specified <paramref name="args"/>.
        /// </summary>
        /// <param name="args">The arguments used to create the process.</param>
        /// <returns>An <see cref="IProcess"/> wrapping a <see cref="System.Diagnostics.Process"/>.</returns>
        IProcess Create(ProcessArguments? args = null);
    }
}
