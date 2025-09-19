namespace Accounting.Application.FixedAssets.Create;

public sealed class CreateFixedAssetCommandValidator : AbstractValidator<CreateFixedAssetCommand>
{
    public CreateFixedAssetCommandValidator()
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
            .MaximumLength(100);

        RuleFor(x => x.Location)
            .MaximumLength(256);

        RuleFor(x => x.Department)
            .MaximumLength(100);
    }
}
