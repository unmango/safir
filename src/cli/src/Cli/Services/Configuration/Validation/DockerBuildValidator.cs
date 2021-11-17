using FluentValidation;

namespace Cli.Services.Configuration.Validation
{
    internal class DockerBuildValidator : AbstractValidator<ServiceSource>
    {
        public DockerBuildValidator()
        {
            RuleFor(x => x.Type).Equal(SourceType.DockerBuild);
            RuleFor(x => x.BuildContext).NotNull().NotEmpty();
            
            RuleSet("Optional", () => {
                RuleFor(x => x.Tag).NotEmpty().WithSeverity(Severity.Info);
            });
        }
    }
}
