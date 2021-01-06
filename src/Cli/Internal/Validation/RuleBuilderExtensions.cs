using FluentValidation;

namespace Cli.Internal.Validation
{
    internal static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string?> ValidUrl<T>(this IRuleBuilder<T, string?> builder)
            => builder.SetValidator(new UrlValidator());
    }
}
