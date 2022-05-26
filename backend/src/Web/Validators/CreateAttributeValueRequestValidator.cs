using FluentValidation;
using Web.Contracts.V1.Requests;

namespace Web.Validators
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
