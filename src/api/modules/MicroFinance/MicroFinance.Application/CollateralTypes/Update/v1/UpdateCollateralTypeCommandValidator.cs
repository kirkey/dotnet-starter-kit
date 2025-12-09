using FluentValidation;
using FSH.Starter.WebApi.MicroFinance.Domain;

namespace FSH.Starter.WebApi.MicroFinance.Application.CollateralTypes.Update.v1;

/// <summary>
/// Validator for UpdateCollateralTypeCommand.
/// </summary>
public class UpdateCollateralTypeCommandValidator : AbstractValidator<UpdateCollateralTypeCommand>
{
    public UpdateCollateralTypeCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("Collateral type ID is required.");

        RuleFor(c => c.Name)
            .MaximumLength(CollateralType.NameMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.Name));

        RuleFor(c => c.Description)
            .MaximumLength(CollateralType.DescriptionMaxLength)
            .When(c => !string.IsNullOrWhiteSpace(c.Description));

        RuleFor(c => c.DefaultLtvPercent)
            .InclusiveBetween(0, 100)
            .When(c => c.DefaultLtvPercent.HasValue)
            .WithMessage("Default LTV percent must be between 0 and 100.");

        RuleFor(c => c.MaxLtvPercent)
            .InclusiveBetween(0, 100)
            .When(c => c.MaxLtvPercent.HasValue)
            .WithMessage("Max LTV percent must be between 0 and 100.");

        RuleFor(c => c.DefaultUsefulLifeYears)
            .GreaterThan(0)
            .When(c => c.DefaultUsefulLifeYears.HasValue)
            .WithMessage("Default useful life must be greater than 0 years.");

        RuleFor(c => c.AnnualDepreciationRate)
            .InclusiveBetween(0, 100)
            .When(c => c.AnnualDepreciationRate.HasValue)
            .WithMessage("Annual depreciation rate must be between 0 and 100.");

        RuleFor(c => c.RevaluationFrequencyMonths)
            .GreaterThan(0)
            .When(c => c.RevaluationFrequencyMonths.HasValue)
            .WithMessage("Revaluation frequency must be greater than 0 months.");

        RuleFor(c => c.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .When(c => c.DisplayOrder.HasValue)
            .WithMessage("Display order must be non-negative.");
    }
}
