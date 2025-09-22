namespace Accounting.Application.BudgetDetails.Update;

public sealed class UpdateBudgetDetailValidator : AbstractValidator<UpdateBudgetDetailCommand>
{
    public UpdateBudgetDetailValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BudgetedAmount)
            .GreaterThanOrEqualTo(0)
            .When(x => x.BudgetedAmount.HasValue);
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}

