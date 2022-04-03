using System;
using FluentValidation;
using FluentValidation.Validators;

namespace Safir.Cli.Internal.Validation
{
    public class UrlValidator<T> : PropertyValidator<T, string?>
    {
        public override string Name => "UrlValidator";

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "{PropertyName} must be a valid url.";
        }

        public override bool IsValid(ValidationContext<T> context, string? value)
        {
            return Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute);
        }
    }
}
