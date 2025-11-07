namespace Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

public sealed class RecordDiscountLostCommandValidator : AbstractValidator<RecordDiscountLostCommand>
{
    public RecordDiscountLostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AP account ID is required.");
        RuleFor(x => x.DiscountAmount).GreaterThan(0).WithMessage("Discount amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
    }
}

