using System;
using System.Diagnostics;

namespace Cli.Internal
{
    internal static class ProcessFactoryExtensions
    {
        public static IProcess Create(this IProcessFactory factory, Action<ProcessStartInfo> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));
            
            var startInfo = new ProcessStartInfo();

            configure(startInfo);

            return Create(factory, new ProcessArguments(null, startInfo));
        }

        public static IProcess Create(this IProcessFactory factory, int id)
            => Create(factory, new ProcessArguments(id));

        private static IProcess Create(this IProcessFactory factory, ProcessArguments? args = null)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            
            return factory.Create(args);
        }
    }
}
