using Accounting.Application.Budgets.BudgetLines.Commands;

namespace Accounting.Application.Budgets.BudgetLines.Validators;

public class AddBudgetLineCommandValidator : AbstractValidator<AddBudgetLineCommand>
{
    public AddBudgetLineCommandValidator()
    {
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.BudgetedAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Description).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description));
    }
}

