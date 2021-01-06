using Cli.Internal.Validation;
using FluentValidation;

namespace Cli.Services.Sources.Validation
{
    internal sealed class GitValidator : AbstractValidator<ServiceSource>
    {
        public GitValidator()
        {
            RuleFor(x => x.Type).Equal(SourceType.Git);
            RuleFor(x => x.CloneUrl)
                .NotNull()
                .NotEmpty()
                .ValidUrl()
                .Must(x => x?.EndsWith(".git") ?? false);
        }
    }
}
