using System.IO;
using FluentValidation;

namespace Safir.Cli.Services.Configuration.Validation;

internal sealed class LocalDirectoryValidator : AbstractValidator<ServiceSource>
{
    public LocalDirectoryValidator()
    {
        RuleFor(x => x.Type).Equal(SourceType.LocalDirectory);
        RuleFor(x => x.SourceDirectory)
            .NotNull()
            .NotEmpty()
            .Must(Directory.Exists); // TODO: Abstract for mocking probably
    }
}