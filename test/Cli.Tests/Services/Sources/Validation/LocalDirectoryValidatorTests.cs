using System.Collections.Generic;
using Cli.Services;
using Cli.Services.Sources.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Cli.Tests.Services.Sources.Validation
{
    public class LocalDirectoryValidatorTests
    {
        private readonly LocalDirectoryValidator _validator = new();
        
        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.LocalDirectory)]
        public void RequiresLocalDirectorySourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            var result = _validator.TestValidate(source);

            result.ShouldHaveValidationErrorFor(x => x.Type);
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStrings))]
        [InlineData("/PleaseDontExistOnYourMachine")]
        public void RequiresSourceDirectory(string? sourceDirectory)
        {
            var source = new ServiceSource {
                Type = SourceType.LocalDirectory,
                SourceDirectory = sourceDirectory,
            };

            var result = _validator.TestValidate(source);

            result.ShouldHaveValidationErrorFor(x => x.SourceDirectory);
        }

        [Fact]
        public void ValidatesForLocalDirectory()
        {
            var source = new ServiceSource {
                Type = SourceType.LocalDirectory,
                // TODO: This only works on Linux machines
                SourceDirectory = "/home",
            };

            var result = _validator.TestValidate(source);

            result.ShouldNotHaveAnyValidationErrors();
        }

        private static IEnumerable<object[]> SourceTypeValuesExcept(SourceType type) => SourceTypeValues.Except(type);
    }
}
