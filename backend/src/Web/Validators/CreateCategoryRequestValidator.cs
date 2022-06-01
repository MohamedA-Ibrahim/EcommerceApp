using FluentValidation;
using Web.Contracts.V1.Requests;

namespace Web.Validators
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
