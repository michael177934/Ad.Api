using Ad.API.Resources.User;
using Ad.Core.Models;
using FluentValidation;


namespace Ad.Api.Validators
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator() 
        {
            RuleFor(x => x.RecipientAccountId).NotEmpty().Matches("^/d{10}$").WithMessage("Recepient Account Number must be greater or less 10 digits");
            RuleFor(x => x.SenderAccountId).NotEmpty().Matches("^/d{10}$").WithMessage("Sender Account Number must be greater or less 10 digits");
            RuleFor(x => x.Amount).NotEmpty();
            //RuleFor(x => x.Balance).NotEmpty();
            //RuleFor(x => x.Role).NotEmpty();
        }
    }
}
