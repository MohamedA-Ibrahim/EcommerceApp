using FluentValidation;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Validators
{
    public class CreateAttributeTypeRequestValidator : AbstractValidator<CreateAttributeTypeRequest>
    {
        public CreateAttributeTypeRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x=> x.CategoryId).NotEmpty();
        }
    }
}
