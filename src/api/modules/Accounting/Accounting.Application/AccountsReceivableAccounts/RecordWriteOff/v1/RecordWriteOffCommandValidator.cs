namespace Accounting.Application.AccountsReceivableAccounts.RecordWriteOff.v1;

public sealed class RecordWriteOffCommandValidator : AbstractValidator<RecordWriteOffCommand>
{
    public RecordWriteOffCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("AR account ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Write-off amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
    }
}

