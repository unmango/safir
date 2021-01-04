using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;

namespace Cli.Middleware
{
    internal static class OptionsValidation
    {
        public static CommandLineBuilder HandleOptionsValidation(this CommandLineBuilder builder)
            => builder.UseMiddleware(async (context, next) => {
                try
                {
                    await next(context);
                }
                catch (Exception e) when (Unwrap(e, out var validationException))
                {
                    var error = context.Console.Error;
                    error.WriteLine($"Options Validation Error: {validationException.OptionsType}");
                    error.WriteLine($"\t{validationException.Message.Replace(";", Environment.NewLine)}");
                }
            }, MiddlewareOrder.ExceptionHandler);

        private static bool Unwrap(
            Exception exception,
            [MaybeNullWhen(false)] out OptionsValidationException validationException)
        {
            validationException = null;

            while (true)
            {
                switch (exception.InnerException)
                {
                    case null:
                        return false;
                    case OptionsValidationException e:
                        validationException = e;
                        return true;
                    default:
                        exception = exception.InnerException;
                        break;
                }
            }
        }
    }
}
