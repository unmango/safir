using System.Collections.Generic;
using FluentValidation.TestHelper;
using Safir.Cli.Services.Configuration;
using Safir.Cli.Services.Configuration.Validation;
using Safir.Cli.Tests.Helpers;
using Xunit;

namespace Safir.Cli.Tests.Services.Configuration.Validation;

public class GitValidatorTests
{
    private readonly GitValidator _validator = new();
        
    [Theory]
    [MemberData(nameof(SourceTypeValuesExcept), SourceType.Git)]
    public void RequiresGitSourceType(SourceType type)
    {
        var source = new ServiceSource { Type = type };

        var result = _validator.TestValidate(source);

        result.ShouldHaveValidationErrorFor(x => x.Type);
    }

    [Theory]
    [ClassData(typeof(NullOrWhitespaceStrings))]
    [InlineData("example.com")]
    [InlineData("https://example.com")]
    [InlineData("https://example.com/path")]
    [InlineData("https://example.com/path?query")]
    [InlineData("https://subdomain.example.com")]
    public void RequiresCloneUrl(string? cloneUrl)
    {
        var source = new ServiceSource {
            Type = SourceType.Git,
            CloneUrl = cloneUrl,
        };

        var result = _validator.TestValidate(source);

        result.ShouldHaveValidationErrorFor(x => x.CloneUrl);
    }

    [Fact]
    public void ValidatesForGit()
    {
        var source = new ServiceSource {
            Type = SourceType.Git,
            CloneUrl = "https://github.com/unmango/safir-cli.git",
        };

        var result = _validator.TestValidate(source);

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static IEnumerable<object[]> SourceTypeValuesExcept(SourceType type) => SourceTypeValues.Except(type);
}