using FluentValidation;
using UserService.BLL.Exceptions;
using UserService.Contracts.DTOs;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");


            RuleFor(x => x)
                .CustomAsync(async (dto, context, ct) =>
                {
                    var userWithEmail = await userRepository.GetByEmailAsync(dto.Email, ct);
                    if (userWithEmail != null)
                    {
                        context.AddFailure("Email", $"User with email : {userWithEmail.Email} is already exist.");
                        return;
                    }
                });
        }

    }
}
