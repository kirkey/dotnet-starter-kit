namespace Accounting.Application.AccountsPayableAccounts.RecordPayment.v1;

public sealed class RecordAPPaymentCommandValidator : AbstractValidator<RecordAPPaymentCommand>
{
    public RecordAPPaymentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AP account ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Payment amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
        RuleFor(x => x.DiscountAmount).GreaterThanOrEqualTo(0).WithMessage("Discount amount must be non-negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Discount amount must not exceed 999,999,999.99.");
    }
}

