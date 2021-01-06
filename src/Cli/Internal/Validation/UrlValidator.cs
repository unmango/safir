using System;
using FluentValidation.Validators;

namespace Cli.Internal.Validation
{
    public class UrlValidator : PropertyValidator
    {
        public UrlValidator() : base("{PropertyName} must be a valid url.")
        { }
        
        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue is string value
                   && Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute);
        }
    }
}
