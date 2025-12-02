namespace Accounting.Application.Budgets.Update;

public sealed class UpdateBudgetCommandValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.FiscalYear)
            .InclusiveBetween(1900, 2100);

        RuleFor(x => x.Name)
            .MaximumLength(256);

        RuleFor(x => x.BudgetType)
            .MaximumLength(32);

        RuleFor(x => x.Status)
            .MaximumLength(16);

        RuleFor(x => x.Description)
            .MaximumLength(1024);

        RuleFor(x => x.Notes)
            .MaximumLength(1024);
    }
}
