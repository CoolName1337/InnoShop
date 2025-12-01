using FluentValidation;
using ProductService.Contracts.DTOs;

namespace ProductService.BLL.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDTO>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0).WithMessage("Count must be greater than or equal to 0.")
                .When(x => x.Count.HasValue);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.")
                .When(x => x.Price.HasValue);
        }
    }
}
