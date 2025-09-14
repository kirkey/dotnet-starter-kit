using Accounting.Application.Budgets.BudgetLines.Commands;

namespace Accounting.Application.Budgets.BudgetLines.Validators;

public class UpdateBudgetLineCommandValidator : AbstractValidator<UpdateBudgetLineCommand>
{
    public UpdateBudgetLineCommandValidator()
    {
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.BudgetedAmount).GreaterThanOrEqualTo(0).When(x => x.BudgetedAmount.HasValue);
        RuleFor(x => x.Description).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description));
    }
}

