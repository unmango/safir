using FluentValidation;

namespace Cli.Services.Sources.Validation
{
    internal sealed class DotnetToolValidator : AbstractValidator<ServiceSource>
    {
        public DotnetToolValidator()
        {
            RuleFor(x => x.Type).Equal(SourceType.DotnetTool);
            RuleFor(x => x.ToolName).NotNull().NotEmpty();
        }
    }
}
