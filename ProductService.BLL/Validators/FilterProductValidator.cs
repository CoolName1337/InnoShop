using FluentValidation;
using ProductService.Contracts.DTOs;

namespace ProductService.BLL.Validators
{
    public class FilterProductValidator : AbstractValidator<FilterProductDTO>
    {
        public FilterProductValidator()
        {
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("MinPrice must be greater than or equal to 0.")
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0).WithMessage("MaxPrice must be greater than or equal to 0.")
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x => x)
                .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
                .WithMessage("MinPrice must be less than or equal to MaxPrice.");

            RuleFor(x => x)
                .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || x.FromDate <= x.ToDate)
                .WithMessage("FromDate must be less than or equal to ToDate.");

            RuleFor(x => x.OwnerId)
                .GreaterThan(0).WithMessage("OwnerId must be greater than 0.")
                .When(x => x.OwnerId.HasValue);
        }
    }
}
