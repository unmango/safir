using System;
using System.Diagnostics;

namespace Cli.Internal.Wrappers.Process
{
    /// <summary>
    /// An abstraction for <see cref="System.Diagnostics.Process"/>.
    /// </summary>
    internal interface IProcess : IDisposable
    {
        /// <summary>
        /// Gets the unique identifier for the associated process.
        /// </summary>
        int Id { get; }
        
        /// <summary>
        /// Gets the properties to pass to the <see cref="Start"/> method of the <see cref="IProcess"/>.
        /// </summary>
        ProcessStartInfo StartInfo { get; }

        /// <summary>
        /// Starts (or reuses) the process resource that is specified by the StartInfo property of
        /// of this <see cref="IProcess"/> component and associates it with the component.
        /// </summary>
        /// <returns></returns>
        bool Start();
    }
}
