namespace Accounting.Application.Budgets.Create;

public sealed class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.PeriodId)
            .NotEmpty();

        RuleFor(x => x.FiscalYear)
            .InclusiveBetween(1900, 2100);

        RuleFor(x => x.BudgetType)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Notes)
            .MaximumLength(1000);
    }
}
