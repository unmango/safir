using System.Collections.Generic;
using Cli.Services;
using Cli.Services.Sources.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Cli.Tests.Services.Sources.Validation
{
    public class DotnetToolValidatorTests
    {
        private readonly DotnetToolValidator _validator = new();
        
        [Theory]
        [MemberData(nameof(SourceTypeValuesExcept), SourceType.DotnetTool)]
        public void RequiresDotnetToolSourceType(SourceType type)
        {
            var source = new ServiceSource { Type = type };

            var result = _validator.TestValidate(source);

            result.ShouldHaveValidationErrorFor(x => x.Type);
        }

        [Theory]
        [ClassData(typeof(NullOrWhitespaceStrings))]
        public void RequiresToolName(string? toolName)
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = toolName,
            };

            var result = _validator.TestValidate(source);

            result.ShouldHaveValidationErrorFor(x => x.ToolName);
        }

        [Fact]
        public void ValidatesForDotnetTool()
        {
            var source = new ServiceSource {
                Type = SourceType.DotnetTool,
                ToolName = "name",
            };

            var result = _validator.TestValidate(source);

            result.ShouldNotHaveAnyValidationErrors();
        }

        private static IEnumerable<object[]> SourceTypeValuesExcept(SourceType type) => SourceTypeValues.Except(type);
    }
}
