using Ad.API.Resources.Auth;
using FluentValidation;

namespace Ad.API.Validators
{
    public class LoginValidator : AbstractValidator<LoginResource>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}