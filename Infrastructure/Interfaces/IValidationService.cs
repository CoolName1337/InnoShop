using FluentValidation.Results;

namespace Infrastructure.Interfaces
{
    public interface IValidationService
    {
        Task<ValidationResult> ValidateAsync<T>(T instance);
    }
}