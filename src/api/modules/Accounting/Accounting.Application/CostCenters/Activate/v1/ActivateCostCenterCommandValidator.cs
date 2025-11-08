namespace Accounting.Application.CostCenters.Activate.v1;

public sealed class ActivateCostCenterCommandValidator : AbstractValidator<ActivateCostCenterCommand>
{
    public ActivateCostCenterCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cost center ID is required.");
    }
}

