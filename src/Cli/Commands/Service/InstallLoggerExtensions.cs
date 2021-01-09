using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Cli.Commands.Service
{
    internal static class InstallLoggerExtensions
    {
        private static readonly Action<ILogger, IEnumerable<string>, Exception?> _servicesToInstall =
            LoggerMessage.Define<IEnumerable<string>>(
                LogLevel.Debug,
                new EventId(1, nameof(ServicesToInstall)),
                "Selected services to install: {Services}");

        private static readonly Action<ILogger, IEnumerable<string>, Exception?> _unmatchedServices =
            LoggerMessage.Define<IEnumerable<string>>(
                LogLevel.Debug,
                new EventId(2, nameof(UnmatchedServices)),
                "Services not matched: {Services}");

        private static readonly Action<ILogger, Exception?> _installationSucceeded =
            LoggerMessage.Define(
                LogLevel.Information,
                new EventId(3, nameof(InstallationSucceeded)),
                "Installation succeeded");

        private static readonly Action<ILogger, Exception?> _installationFailed =
            LoggerMessage.Define(
                LogLevel.Error,
                new EventId(4, nameof(InstallationFailed)),
                "Installation failed");


        public static void ServicesToInstall(this ILogger logger, IEnumerable<string> services)
        {
            _servicesToInstall(logger, services, null);
        }

        public static void UnmatchedServices(this ILogger logger, IEnumerable<string> services)
        {
            _unmatchedServices(logger, services, null);
        }

        public static void InstallationSucceeded(this ILogger logger)
        {
            _installationSucceeded(logger, null);
        }

        public static void InstallationFailed(this ILogger logger, Exception exception)
        {
            _installationFailed(logger, exception);
        }
    }
}
