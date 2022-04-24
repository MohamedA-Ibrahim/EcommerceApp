using FluentValidation;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Validators
{
    public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
            RuleFor(x => x.Price).NotEmpty();
        }
    }
}