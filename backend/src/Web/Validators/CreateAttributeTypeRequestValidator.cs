using FluentValidation;
using Web.Contracts.V1.Requests;

namespace Web.Validators
{
    public class CreateAttributeTypeRequestValidator : AbstractValidator<CreateAttributeTypeRequest>
    {
        public CreateAttributeTypeRequestValidator()
        {
            RuleFor(x=> x.CategoryId).NotEmpty();
        }
    }
}
