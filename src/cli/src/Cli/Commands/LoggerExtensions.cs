using System;
using Microsoft.Extensions.Logging;

namespace Cli.Commands
{
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, string, bool, Exception?> _boolOption =
            Option<bool>(new EventId(1, nameof(BoolOption)));

        private static readonly Action<ILogger, string, int, Exception?> _intOption =
            Option<int>(new EventId(2, nameof(IntOption)));

        private static readonly Action<ILogger, string, object, Exception?> _option =
            Option<object>(new EventId(3, nameof(Option)));

        public static void BoolOption(this ILogger logger, string name, bool value)
        {
            _boolOption(logger, name, value, null);
        }

        public static void IntOption(this ILogger logger, string name, int value)
        {
            _intOption(logger, name, value, null);
        }

        public static void Option(this ILogger logger, string name, object value)
        {
            _option(logger, name, value, null);
        }

        private static Action<ILogger, string, T, Exception?> Option<T>(EventId eventId)
            => LoggerMessage.Define<string, T>(
                LogLevel.Trace,
                eventId,
                "Invoked with option: {Name} = {Value}");
    }
}
