using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Cli.Services.Dotnet
{
    internal static class DotnetCommandExtensions
    {
        private static readonly Dictionary<DotnetCommand, string> _nameMap = new() {
            { DotnetCommand.Run, "run" },
            { DotnetCommand.Tool, "tool" }
        };

        public static string GetCommand(this DotnetCommand command)
        {
            if (!TryGetCommand(command, out var name))
            {
                throw new NotSupportedException($"Command {command} is not supported");
            }

            return name;
        }

        public static bool TryGetCommand(this DotnetCommand command, [MaybeNullWhen(false)] out string name)
        {
            return _nameMap.TryGetValue(command, out name);
        }
    }
}
