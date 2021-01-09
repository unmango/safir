using System;
using Cli.Services;
using Microsoft.Extensions.Logging;

namespace Cli.Internal
{
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, string?, Exception?> _invokedWorkingDirectory =
            LoggerMessage.Define<string?>(
                LogLevel.Debug,
                new EventId(1, nameof(InvokedWorkingDirectory)),
                "Install invoked with directory: {Directory}");
        
        private static readonly Action<ILogger, string, Exception?> _resolvedWorkingDirectory =
            LoggerMessage.Define<string>(
                LogLevel.Debug,
                new EventId(2, nameof(ResolvedWorkingDirectory)),
                "Install resolved directory: {Directory}");

        private static readonly Action<ILogger, InstallationContext, Exception?> _initialContextCreated =
            LoggerMessage.Define<InstallationContext>(
                LogLevel.Trace,
                new EventId(3, nameof(InitialContextCreated)),
                "Initial installation context created: {Context}");
        
        public static void InvokedWorkingDirectory(this ILogger logger, string? directory)
        {
            _invokedWorkingDirectory(logger, directory, null);
        }

        public static void ResolvedWorkingDirectory(this ILogger logger, string directory)
        {
            _resolvedWorkingDirectory(logger, directory, null);
        }

        public static void InitialContextCreated(this ILogger logger, InstallationContext context)
        {
            _initialContextCreated(logger, context, null);
        }
    }
}
