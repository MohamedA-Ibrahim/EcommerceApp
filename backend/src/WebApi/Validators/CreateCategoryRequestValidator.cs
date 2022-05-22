using FluentValidation;
using WebApi.Contracts.V1.Requests;

namespace WebApi.Validators
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
