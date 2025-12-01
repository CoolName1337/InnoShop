using FluentValidation;
using Infrastructure.Interfaces;
using UserService.Contracts.DTOs;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Validators
{
    public class LoginValidator : AbstractValidator<LoginUserDTO>
    {
        public LoginValidator(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");


            RuleFor(x => x)
                .CustomAsync(async (dto, context, ct) =>
                {
                    var user = await userRepository.GetByEmailAsync(dto.Email, ct);
                    if (user == null)
                    {
                        context.AddFailure("Email", "User with this email was not found.");
                        return;
                    }
                    if (!user.IsEmailConfirmed)
                    {
                        context.AddFailure("Email", "Email has not been confirmed.");
                    }
                    if (!passwordHasher.Verify(dto.Password, user.PasswordHash))
                    { 
                        context.AddFailure("Password", "Incorrect password."); 
                    }
                });
        }

    }
}
