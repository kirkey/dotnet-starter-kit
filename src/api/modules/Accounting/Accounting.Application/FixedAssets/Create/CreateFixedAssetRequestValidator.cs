using Accounting.Domain.Constants;

namespace Accounting.Application.FixedAssets.Create;

public sealed class CreateFixedAssetCommandValidator : AbstractValidator<CreateFixedAssetCommand>
{
    public CreateFixedAssetCommandValidator()
    {
        RuleFor(x => x.AssetName)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.PurchaseDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1)) // allow slight clock skew
            .WithMessage("Purchase date cannot be in the far future.");

        RuleFor(x => x.PurchasePrice)
            .GreaterThan(0);

        RuleFor(x => x.DepreciationMethodId)
            .NotEmpty();

        RuleFor(x => x.ServiceLife)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.SalvageValue)
            .GreaterThanOrEqualTo(0)
            .LessThan(x => x.PurchasePrice);

        RuleFor(x => x.AccumulatedDepreciationAccountId)
            .NotEmpty();

        RuleFor(x => x.DepreciationExpenseAccountId)
            .NotEmpty();

        RuleFor(x => x.AssetType)
            .NotEmpty()
            .Must(FixedAssetTypes.Contains)
            .WithMessage(_ => $"AssetType must be one of: {FixedAssetTypes.AsDisplayList()}");

        RuleFor(x => x.SerialNumber)
            .MaximumLength(128);

        RuleFor(x => x.Location)
            .MaximumLength(256);

        RuleFor(x => x.Department)
            .MaximumLength(128);

        RuleFor(x => x.Description)
            .MaximumLength(1024);

        RuleFor(x => x.Notes)
            .MaximumLength(1024);

        RuleFor(x => x.GpsCoordinates)
            .MaximumLength(128);

        RuleFor(x => x.SubstationName)
            .MaximumLength(256);

        RuleFor(x => x.RegulatoryClassification)
            .MaximumLength(256);

        RuleFor(x => x.VoltageRating)
            .GreaterThan(0)
            .When(x => x.VoltageRating.HasValue);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .When(x => x.Capacity.HasValue);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(256);

        RuleFor(x => x.ModelNumber)
            .MaximumLength(128);
    }
}
