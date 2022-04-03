using FluentValidation;
using Safir.Cli.Internal.Validation;

namespace Safir.Cli.Services.Configuration.Validation;

internal sealed class GitValidator : AbstractValidator<ServiceSource>
{
    public GitValidator()
    {
        RuleFor(x => x.Type).Equal(SourceType.Git);
        RuleFor(x => x.CloneUrl).NotEmpty().ValidUrl();
        RuleFor(x => x.CloneUrl).Must(x => x?.EndsWith(".git") ?? false)
            .WithMessage("Must end with \".git\"");
    }
}