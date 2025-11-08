namespace Accounting.Application.AccountsPayableAccounts.RecordDiscountLost.v1;

/// <summary>
/// Validator for RecordAPDiscountLostCommand.
/// </summary>
public sealed class RecordAPDiscountLostCommandValidator : AbstractValidator<RecordAPDiscountLostCommand>
{
    public RecordAPDiscountLostCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AP account ID is required.");
        RuleFor(x => x.DiscountAmount).GreaterThan(0).WithMessage("Discount amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
    }
}

