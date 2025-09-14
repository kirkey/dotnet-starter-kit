namespace Accounting.Application.FixedAssets.Create;

public class CreateFixedAssetRequestValidator : AbstractValidator<CreateFixedAssetRequest>
{
    public CreateFixedAssetRequestValidator()
    {
        RuleFor(x => x.AssetName)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.PurchaseDate)
            .NotEmpty();

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0);

        RuleFor(x => x.DepreciationMethodId)
            .NotEmpty();

        RuleFor(x => x.ServiceLife)
            .GreaterThan(0);

        RuleFor(x => x.SalvageValue)
            .GreaterThanOrEqualTo(0)
            .LessThan(x => x.PurchasePrice);

        RuleFor(x => x.AccumulatedDepreciationAccountId)
            .NotEmpty();

        RuleFor(x => x.DepreciationExpenseAccountId)
            .NotEmpty();

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.SerialNumber));

        RuleFor(x => x.Location)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Department));
    }
}
