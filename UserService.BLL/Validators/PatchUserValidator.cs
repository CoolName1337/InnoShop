using FluentValidation;
using UserService.Contracts.DTOs;

namespace UserService.BLL.Validators
{
    public class PatchUserValidator : AbstractValidator<PatchUserDTO>
    {
        public PatchUserValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));
        }

    }
}
