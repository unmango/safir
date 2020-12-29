using System;
using Microsoft.Extensions.Logging;

namespace Cli.Services
{
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, Type, Exception?> _startInvoked =
            LoggerMessage.Define<Type>(
                LogLevel.Trace,
                new EventId(1, nameof(StartInvoked)),
                "StartAsync invoked for service {Type}");

        private static readonly Action<ILogger, int, Exception?> _processCreated =
            LoggerMessage.Define<int>(
                LogLevel.Information,
                new EventId(2, nameof(ProcessCreated)),
                "Process created with id {ProcessId}");

        private static readonly Action<ILogger, int, bool, Exception?> _processStarted =
            LoggerMessage.Define<int, bool>(
                LogLevel.Information,
                new EventId(3, nameof(ProcessStarted)),
                "Process {ProcessId} started with result {Result}");

        public static void StartInvoked<T>(this ILogger logger) where T : IService
        {
            logger.StartInvoked(typeof(T));
        }

        public static void StartInvoked(this ILogger logger, Type type)
        {
            _startInvoked(logger, type, null);
        }

        public static void ProcessCreated(this ILogger logger, int processId)
        {
            _processCreated(logger, processId, null);
        }

        public static void ProcessStarted(this ILogger logger, int processId, bool result)
        {
            _processStarted(logger, processId, result, null);
        }
    }
}
