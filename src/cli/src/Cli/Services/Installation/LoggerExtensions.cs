using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Cli.Services.Installation
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
        
        private static readonly Action<ILogger, IEnumerable<string>, Exception?> _initialInstallers =
            LoggerMessage.Define<IEnumerable<string>>(
                LogLevel.Trace,
                new EventId(4, nameof(InitialInstallers)),
                "Initial installers: {Installers}");
        
        private static readonly Action<ILogger, IEnumerable<string>, Exception?> _applicableInstallers =
            LoggerMessage.Define<IEnumerable<string>>(
                LogLevel.Debug,
                new EventId(5, nameof(ApplicableInstallers)),
                "Applicable installers: {Installers}");
        
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
        
        public static void InitialInstallers(this ILogger logger, IEnumerable<IInstallationMiddleware> installers)
        {
            _initialInstallers(logger, installers.Select(x => x.GetType().Name), null);
        }
        
        public static void ApplicableInstallers(this ILogger logger, IEnumerable<IInstallationMiddleware> installers)
        {
            _applicableInstallers(logger, installers.Select(x => x.GetType().Name), null);
        }
    }
}
