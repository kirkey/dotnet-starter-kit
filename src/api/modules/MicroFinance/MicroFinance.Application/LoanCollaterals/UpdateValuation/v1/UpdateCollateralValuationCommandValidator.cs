using FluentValidation;

namespace FSH.Starter.WebApi.MicroFinance.Application.LoanCollaterals.UpdateValuation.v1;

/// <summary>
/// Validator for UpdateCollateralValuationCommand.
/// </summary>
public class UpdateCollateralValuationCommandValidator : AbstractValidator<UpdateCollateralValuationCommand>
{
    public UpdateCollateralValuationCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Collateral ID is required.");

        RuleFor(x => x.EstimatedValue)
            .GreaterThan(0).WithMessage("Estimated value must be greater than zero.");

        RuleFor(x => x.ForcedSaleValue)
            .LessThanOrEqualTo(x => x.EstimatedValue)
            .When(x => x.ForcedSaleValue.HasValue)
            .WithMessage("Forced sale value cannot exceed estimated value.");
    }
}
