namespace Accounting.Application.BankReconciliations.Create.v1;

/// <summary>
/// Validator for CreateBankReconciliationCommand.
/// Ensures all reconciliation opening data is valid before persistence.
/// </summary>
public sealed class CreateBankReconciliationRequestValidator : AbstractValidator<CreateBankReconciliationCommand>
{
    public CreateBankReconciliationRequestValidator()
    {
        RuleFor(x => x.BankAccountId)
            .NotEmpty()
            .WithMessage("Bank account is required.")
            .Must(id => id != DefaultIdType.Empty)
            .WithMessage("Bank account ID must be a valid identifier.");

        RuleFor(x => x.ReconciliationDate)
            .NotEmpty()
            .WithMessage("Reconciliation date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Reconciliation date cannot be in the future.");

        RuleFor(x => x.StatementBalance)
            .NotEmpty()
            .WithMessage("Statement balance is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Statement balance cannot be negative.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Statement balance exceeds maximum allowed value.");

        RuleFor(x => x.BookBalance)
            .NotEmpty()
            .WithMessage("Book balance is required.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Book balance cannot be negative.")
            .LessThanOrEqualTo(999999999.99m)
            .WithMessage("Book balance exceeds maximum allowed value.");

        RuleFor(x => x.StatementNumber)
            .MaximumLength(128)
            .WithMessage("Statement number cannot exceed 100 characters.")
            .Matches(@"^[a-zA-Z0-9\-._/]{1,}$")
            .WithMessage("Statement number contains invalid characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.StatementNumber));

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description cannot exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Notes cannot exceed 2048 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
