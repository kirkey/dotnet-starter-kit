using Accounting.Application.DeferredRevenues.Commands;

namespace Accounting.Application.DeferredRevenues.Validators;

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