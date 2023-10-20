using Ad.API.Resources.User;
using FluentValidation;

namespace Ad.Api.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserResource>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches("^[0-9]{11}$").WithMessage("Phone number should be 11 digit");
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}
