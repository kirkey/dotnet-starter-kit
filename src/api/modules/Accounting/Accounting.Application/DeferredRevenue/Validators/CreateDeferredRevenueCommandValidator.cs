using FluentValidation;
using Accounting.Application.DeferredRevenue.Commands;

namespace Accounting.Application.DeferredRevenue.Validators
{
    public class CreateDeferredRevenueCommandValidator : AbstractValidator<CreateDeferredRevenueCommand>
    {
        public CreateDeferredRevenueCommandValidator()
        {
            RuleFor(x => x.DeferredRevenueNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.RecognitionDate).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
        }
    }
}

