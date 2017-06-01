using log4net;
using log4net.Core;
using System.Runtime.CompilerServices;

namespace Safir.Core.Helpers
{
    public class LogHelper
    {
        public static ILog GetLogger([CallerFilePath]string filename = "")
        {
            return LogManager.GetLogger(filename);
        }
    }

    public sealed class Log4NetAdapter<T> : LogImpl
    {
        public Log4NetAdapter() : base(LogManager.GetLogger(typeof(T)).Logger) { }
    }
}
