using FluentValidation;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Validators
{
    public class CreateAttributeValueRequestValidator : AbstractValidator<CreateAttributeValueRequest>
    {
        public CreateAttributeValueRequestValidator()
        {
            RuleFor(x => x.AttributeValue).NotEmpty();
            RuleFor(x => x.AttributeTypeId).NotEmpty();

        }
    }
}
