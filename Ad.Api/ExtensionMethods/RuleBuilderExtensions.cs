using FluentValidation;

namespace Ad.API.ExtensionMethods
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, int minimumLength = 6)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(minimumLength).WithMessage("Passwords must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Passwords must have at least one uppercase ('A'-'Z').")
                .Matches("[a-z]").WithMessage("Passwords must have at least one lowercase ('A'-'Z').")
                .Matches("[0-9]").WithMessage("Passwords must have at least one digit ('0'-'9').")
                .Matches("[^a-zA-Z0-9]").WithMessage("Passwords must have at least one non alphanumeric character.");
        }
    }
}
