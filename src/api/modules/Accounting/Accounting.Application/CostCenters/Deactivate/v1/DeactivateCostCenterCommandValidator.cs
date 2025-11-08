namespace Accounting.Application.CostCenters.Deactivate.v1;

public sealed class DeactivateCostCenterCommandValidator : AbstractValidator<DeactivateCostCenterCommand>
{
    public DeactivateCostCenterCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cost center ID is required.");
    }
}

