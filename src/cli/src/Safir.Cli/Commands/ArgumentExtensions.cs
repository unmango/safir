using System.CommandLine;
using System.CommandLine.Parsing;

namespace Safir.Cli.Commands;

internal static class ArgumentExtensions
{
    public static T WithValidator<T>(this T argument, ValidateSymbolResult<ArgumentResult> validate)
        where T : Argument
    {
        argument.AddValidator(validate);

        return argument;
    }
}
