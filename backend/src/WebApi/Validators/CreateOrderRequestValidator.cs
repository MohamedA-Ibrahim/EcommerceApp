using FluentValidation;
using Web.Contracts.V1.Requests;

namespace Web.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty();
            RuleFor(x=> x.ItemId).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty().Length(11,11);
            RuleFor(x => x.StreetAddress).NotEmpty().MaximumLength(250);
            RuleFor(x => x.City).NotEmpty().MaximumLength(50);
            RuleFor(x => x.State).NotEmpty().MaximumLength(50);
            RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(10);
            RuleFor(x => x.RecieverName).NotEmpty().MaximumLength(150);
        }
    }
}
