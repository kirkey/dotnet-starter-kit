namespace Accounting.Application.BankReconciliations.Update.v1;

/// <summary>
/// Validator for UpdateBankReconciliationCommand.
/// Ensures reconciliation items are valid and non-negative.
/// </summary>
public sealed class UpdateBankReconciliationCommandValidator : AbstractValidator<UpdateBankReconciliationCommand>
{
    public UpdateBankReconciliationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Reconciliation ID is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Reconciliation ID must be a valid identifier.");

        RuleFor(x => x.OutstandingChecksTotal)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Outstanding checks total cannot be negative.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Outstanding checks total exceeds maximum allowed value.");

        RuleFor(x => x.DepositsInTransitTotal)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Deposits in transit total cannot be negative.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Deposits in transit total exceeds maximum allowed value.");

        RuleFor(x => x.BankErrors)
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Bank errors exceeds maximum allowed value.")
            .GreaterThanOrEqualTo(-999999999.99m)
            .WithMessage("Bank errors below minimum allowed value.");

        RuleFor(x => x.BookErrors)
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Book errors exceeds maximum allowed value.")
            .GreaterThanOrEqualTo(-999999999.99m)
            .WithMessage("Book errors below minimum allowed value.");
    }
}

