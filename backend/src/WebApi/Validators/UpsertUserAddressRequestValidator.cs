using FluentValidation;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Validators
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
            RuleFor(x => x.State).NotEmpty().MaximumLength(40);
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(10);
            RuleFor(x => x.RecieverName).NotEmpty().MaximumLength(100);
        }
    }
}
