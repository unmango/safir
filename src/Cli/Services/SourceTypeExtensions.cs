using System;
using System.Collections.Generic;

namespace Cli.Services
{
    internal static class SourceTypeExtensions
    {
        private static readonly IEnumerable<CommandType> _empty = Array.Empty<CommandType>();

        private static readonly Dictionary<SourceType, IEnumerable<CommandType>> _map = new() {
            [SourceType.Git] = new[] { CommandType.DockerRun, CommandType.DotnetRun },
            [SourceType.DotnetTool] = new[] { CommandType.DotnetTool },
            [SourceType.LocalDirectory] = new[] { CommandType.DockerRun, CommandType.DotnetRun },
        };

        public static IEnumerable<CommandType> SupportedCommands(this SourceType sourceType)
        {
            return _map.TryGetValue(sourceType, out var commands)
                ? commands
                : _empty;
        }
    }
}
