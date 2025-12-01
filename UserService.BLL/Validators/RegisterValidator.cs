using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UserService.Contracts.DTOs;

namespace UserService.BLL.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterValidator()
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
        }

    }
}
