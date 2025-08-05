using FluentValidation;

namespace Accounting.Application.FixedAssets.Update;

public class UpdateFixedAssetRequestValidator : AbstractValidator<UpdateFixedAssetRequest>
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
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.SerialNumber));

        RuleFor(x => x.Location)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Department));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
