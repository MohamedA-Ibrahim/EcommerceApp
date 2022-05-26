using FluentValidation;
using Web.Contracts.V1.Requests;

namespace Web.Validators
{
    public class UpsertUserAddressRequestValidator : AbstractValidator<UpsertUserAddressRequest>
    {
        public UpsertUserAddressRequestValidator()
        {
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches("^([0-9]{11})$").WithMessage("Enter a valid mobile number");
            RuleFor(x => x.StreetAddress).NotEmpty().MaximumLength(100);
            RuleFor(x => x.City).NotEmpty().MaximumLength(40);
            RuleFor(x => x.RecieverName).NotEmpty().MaximumLength(100);
        }
    }
}
