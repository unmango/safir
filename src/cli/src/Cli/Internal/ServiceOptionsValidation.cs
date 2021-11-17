using System;
using System.Collections.Generic;
using System.Linq;
using Cli.Services.Configuration;
using Microsoft.Extensions.Options;

namespace Cli.Internal
{
    internal static class ServiceOptionsValidation
    {
        public static OptionsBuilder<ServiceOptions> AddValidators(this OptionsBuilder<ServiceOptions> builder)
        {
            // builder.Services.AddSingleton<IValidateOptions<ServiceOptions>, Sources>();

            return builder;
        }

        // ReSharper disable once UnusedType.Local
        private class Sources : IValidateOptions<ServiceOptions>
        {
            public ValidateOptionsResult Validate(string name, ServiceOptions options)
            {
                var errors = options.Values
                    .SelectMany(x => x.Sources.Select(Validate))
                    .SelectMany(x => x.Failures)
                    .ToList();

                return errors.Any()
                    ? ValidateOptionsResult.Fail(errors)
                    : ValidateOptionsResult.Success;
            }

            private static ValidateOptionsResult Validate(ServiceSource source)
            {
                if (source == null) return ValidateOptionsResult.Skip;

                return source.Type switch {
                    SourceType.Git => ValidateGit(source),
                    SourceType.DotnetTool => ValidateDotnetTool(source),
                    SourceType.LocalDirectory => ValidateLocalDirectory(source),
                    null => ValidateOptionsResult.Fail("Source type not configured"),
                    _ => ValidateOptionsResult.Fail("Invalid source type")
                };
            }

            private static ValidateOptionsResult ValidateGit(ServiceSource source)
                => Errors(
                    (!string.IsNullOrWhiteSpace(source.CloneUrl), "GitCloneUrl is required"),
                    (
                        source.Command != null &&
                        SupportedCommands(source.Type!.Value).Contains(source.Command.Value),
                        "Command is required"
                    ));

            // ReSharper disable once UnusedParameter.Local
            private static ValidateOptionsResult ValidateDotnetTool(ServiceSource source)
            {
                throw new NotImplementedException();
            }

            // ReSharper disable once UnusedParameter.Local
            private static ValidateOptionsResult ValidateLocalDirectory(ServiceSource source)
            {
                throw new NotImplementedException();
            }

            private static ValidateOptionsResult Errors(params (bool result, string messsage)[] results)
            {
                var errors = results.Where(x => !x.result).ToList();

                return errors.Any()
                    ? ValidateOptionsResult.Fail(errors.Select(x => x.messsage))
                    : ValidateOptionsResult.Success;
            }

            private static IEnumerable<CommandType> SupportedCommands(SourceType sourceType)
                => sourceType switch {
                    SourceType.Git => new[] {
                        CommandType.DockerRun,
                        CommandType.DotnetRun
                    },
                    SourceType.DotnetTool => new[] { CommandType.DotnetTool },
                    SourceType.LocalDirectory => new[] {
                        CommandType.DockerRun,
                        CommandType.DotnetRun,
                    },
                    _ => Array.Empty<CommandType>()
                };
        }
    }
}
