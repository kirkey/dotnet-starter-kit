namespace Accounting.Application.AccountsReceivableAccounts.RecordCollection.v1;

/// <summary>
/// Validator for RecordARCollectionCommand.
/// </summary>
public sealed class RecordARCollectionCommandValidator : AbstractValidator<RecordARCollectionCommand>
{
    public RecordARCollectionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AR account ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Collection amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
    }
}

