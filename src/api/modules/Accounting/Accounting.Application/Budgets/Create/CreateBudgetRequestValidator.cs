using FluentValidation;

namespace Accounting.Application.Budgets.Create;

public class CreateBudgetRequestValidator : AbstractValidator<CreateBudgetRequest>
{
    public CreateBudgetRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.PeriodId)
            .NotEmpty();

        RuleFor(x => x.FiscalYear)
            .GreaterThan(1900)
            .LessThanOrEqualTo(2100);

        RuleFor(x => x.BudgetType)
            .NotEmpty()
            .MaximumLength(32);

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
