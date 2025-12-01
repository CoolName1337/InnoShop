using FluentValidation;
using UserService.Contracts.DTOs;
using UserService.DAL.Interfaces;
using UserService.DAL.Repositories;

namespace UserService.BLL.Validators
{
    public class PatchUserValidator : AbstractValidator<PatchUserDTO>
    {
        public PatchUserValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Name)
                .MaximumLength(255).WithMessage("Name must not exceed 255 characters.")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x)
                .CustomAsync(async (dto, context, ct) =>
                {
                    var user = await userRepository.GetByIdAsync(dto.Id, ct);
                    if (user == null)
                    {
                        context.AddFailure("Id", $"User with id = {dto.Id} was not found.");
                        return;
                    }
                });
        }

    }
}
