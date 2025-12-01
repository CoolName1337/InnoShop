using FluentValidation;
using ProductService.Contracts.DTOs;

namespace ProductService.BLL.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDTO>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

            RuleFor(x => x.Description)
               .NotEmpty().WithMessage("Description is required.")
               .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0).WithMessage("Count must be greater than or equal to 0.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");
        }
    }
}
