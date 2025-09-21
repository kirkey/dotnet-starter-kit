using Accounting.Application.Budgets.BudgetDetails.Commands;

namespace Accounting.Application.Budgets.BudgetDetails.Validators;

public class UpdateBudgetDetailCommandValidator : AbstractValidator<UpdateBudgetDetailCommand>
{
    public UpdateBudgetDetailCommandValidator()
    {
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.BudgetedAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Description).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description));
    }
}

