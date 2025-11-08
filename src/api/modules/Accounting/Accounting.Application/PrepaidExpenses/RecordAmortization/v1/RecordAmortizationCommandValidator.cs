namespace Accounting.Application.PrepaidExpenses.RecordAmortization.v1;

public sealed class RecordAmortizationCommandValidator : AbstractValidator<RecordAmortizationCommand>
{
    public RecordAmortizationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Prepaid expense ID is required.");
        RuleFor(x => x.AmortizationAmount).GreaterThan(0).WithMessage("Amortization amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amortization amount must not exceed 999,999,999.99.");
        RuleFor(x => x.PostingDate).NotEmpty().WithMessage("Posting date is required.");
    }
}

