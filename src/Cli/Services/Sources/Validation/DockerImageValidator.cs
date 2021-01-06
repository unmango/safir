using FluentValidation;

namespace Cli.Services.Sources.Validation
{
    internal sealed class DockerImageValidator : AbstractValidator<ServiceSource>
    {
        public DockerImageValidator()
        {
            RuleFor(x => x.Type).Equal(SourceType.DockerImage);
            RuleFor(x => x.ImageName).NotNull().NotEmpty();
            
            RuleSet("Optional", () => {
                RuleFor(x => x.Tag).NotEmpty().WithSeverity(Severity.Info);
            });
        }
    }
}
