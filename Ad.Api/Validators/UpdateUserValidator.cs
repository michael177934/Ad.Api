using Ad.API.Resources.User;
using FluentValidation;

namespace Ad.API.Validators
{
    public class UpdateUserValidator : AbstractValidator<UserResource>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches("^[0-9]{11}$").WithMessage("Phone number should be 11 digit");
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}