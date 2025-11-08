namespace Accounting.Application.FixedAssets.Dispose.v1;

public sealed class DisposeFixedAssetCommandValidator : AbstractValidator<DisposeFixedAssetCommand>
{
    public DisposeFixedAssetCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Fixed asset ID is required.");
        RuleFor(x => x.DisposalDate).NotEmpty().WithMessage("Disposal date is required.");
        RuleFor(x => x.DisposalAmount).GreaterThanOrEqualTo(0).WithMessage("Disposal amount must be non-negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Disposal amount must not exceed 999,999,999.99.")
            .When(x => x.DisposalAmount.HasValue);
        RuleFor(x => x.DisposalReason).MaximumLength(500).WithMessage("Disposal reason must not exceed 500 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.DisposalReason));
    }
}

