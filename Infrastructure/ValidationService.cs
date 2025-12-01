using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public class ValidationService(IServiceProvider serviceProvider) : IValidationService
    {
        public async Task<ValidationResult> ValidateAsync<T>(T instance)
        {
            var validator = serviceProvider.GetService<IValidator<T>>()
                ?? throw new InvalidOperationException($"No validator registered for {typeof(T).Name}");
            var res = await validator.ValidateAsync(instance);

            if (!res.IsValid) {
                throw new ValidationException(res.Errors);
            }

            return res;
        }
    }
}
