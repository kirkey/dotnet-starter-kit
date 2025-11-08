namespace Accounting.Application.WriteOffs.RecordRecovery.v1;

public sealed class RecordRecoveryCommandValidator : AbstractValidator<RecordRecoveryCommand>
{
    public RecordRecoveryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Write-off ID is required.");
        RuleFor(x => x.RecoveryAmount).GreaterThan(0).WithMessage("Recovery amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Recovery amount must not exceed 999,999,999.99.");
    }
}

