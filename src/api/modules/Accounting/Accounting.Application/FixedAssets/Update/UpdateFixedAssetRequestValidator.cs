namespace Accounting.Application.FixedAssets.Update;

public class UpdateFixedAssetRequestValidator : AbstractValidator<UpdateFixedAssetCommand>
{
    public UpdateFixedAssetRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.AssetName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.AssetName));

        RuleFor(x => x.ServiceLife)
            .GreaterThan(0)
            .When(x => x.ServiceLife.HasValue);

        RuleFor(x => x.SalvageValue)
            .GreaterThanOrEqualTo(0)
            .When(x => x.SalvageValue.HasValue);

        RuleFor(x => x.SerialNumber)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.SerialNumber));

        RuleFor(x => x.Location)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Department)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.Department));

        RuleFor(x => x.Description)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.GpsCoordinates)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.GpsCoordinates));

        RuleFor(x => x.SubstationName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.SubstationName));

        RuleFor(x => x.RegulatoryClassification)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.RegulatoryClassification));

        RuleFor(x => x.VoltageRating)
            .GreaterThan(0)
            .When(x => x.VoltageRating.HasValue);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .When(x => x.Capacity.HasValue);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Manufacturer));

        RuleFor(x => x.ModelNumber)
            .MaximumLength(128)
            .When(x => !string.IsNullOrEmpty(x.ModelNumber));
    }
}
