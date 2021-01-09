using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Cli.Services.Installers
{
    internal static class LoggerExtensions
    {
        private static readonly Action<ILogger, IEnumerable<string>, Exception?> _initialInstallers =
            LoggerMessage.Define<IEnumerable<string>>(
                LogLevel.Trace,
                new EventId(1, nameof(InitialInstallers)),
                "Initial installers: {Installers}");
        
        private static readonly Action<ILogger, IEnumerable<string>, Exception?> _applicableInstallers =
            LoggerMessage.Define<IEnumerable<string>>(
                LogLevel.Debug,
                new EventId(2, nameof(ApplicableInstallers)),
                "Applicable installers: {Installers}");
        
        public static void InitialInstallers(this ILogger logger, IEnumerable<IPipelineServiceInstaller> installers)
        {
            _initialInstallers(logger, installers.Select(x => x.GetType().Name), null);
        }
        
        public static void ApplicableInstallers(this ILogger logger, IEnumerable<IPipelineServiceInstaller> installers)
        {
            _applicableInstallers(logger, installers.Select(x => x.GetType().Name), null);
        }
    }
}
