namespace Accounting.Application.CostCenters.UpdateBudget.v1;

/// <summary>
/// Validator for UpdateCostCenterBudgetCommand.
/// </summary>
public sealed class UpdateCostCenterBudgetCommandValidator : AbstractValidator<UpdateCostCenterBudgetCommand>
{
    public UpdateCostCenterBudgetCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Cost center ID is required.");
        RuleFor(x => x.BudgetAmount).GreaterThanOrEqualTo(0).WithMessage("Budget amount cannot be negative.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Budget amount must not exceed 999,999,999.99.");
    }
}

