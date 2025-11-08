namespace Accounting.Application.FixedAssets.Depreciate;

public sealed class DepreciateFixedAssetCommandValidator : AbstractValidator<DepreciateFixedAssetCommand>
{
    public DepreciateFixedAssetCommandValidator()
    {
        RuleFor(x => x.FixedAssetId).NotEmpty().WithMessage("Fixed asset ID is required.");
        RuleFor(x => x.DepreciationAmount).GreaterThan(0).WithMessage("Depreciation amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Depreciation amount must not exceed 999,999,999.99.");
        RuleFor(x => x.DepreciationDate).NotEmpty().WithMessage("Depreciation date is required.");
        RuleFor(x => x.DepreciationMethod).MaximumLength(100).WithMessage("Depreciation method must not exceed 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.DepreciationMethod));
    }
}

