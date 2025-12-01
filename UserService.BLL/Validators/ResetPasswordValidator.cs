using FluentValidation;
using UserService.Contracts.DTOs;

namespace UserService.BLL.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordDTO>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.NewPassword)
               .NotEmpty().WithMessage("NewPassword is required.")
               .MinimumLength(6).WithMessage("NewPassword must be at least 6 characters long.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token is required.");
        }
    }
}
