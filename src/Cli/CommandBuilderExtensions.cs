using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;

namespace Cli
{
    internal static class CommandBuilderExtensions
    {
        public static T AddAlias<T>(this T builder, string alias)
            where T : CommandBuilder
        {
            builder.Command.AddAlias(alias);

            return builder;
        }

        public static T AddHandler<T>(this T builder, ICommandHandler handler)
            where T : CommandBuilder
        {
            builder.Command.Handler = handler;
            
            return builder;
        }
    }
}
