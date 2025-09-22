namespace Accounting.Application.BudgetDetails.Create;

public sealed class CreateBudgetDetailValidator : AbstractValidator<CreateBudgetDetailCommand>
{
    public CreateBudgetDetailValidator()
    {
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.BudgetedAmount).GreaterThanOrEqualTo(0m);
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
