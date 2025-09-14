namespace Accounting.Application.Budgets.Update;

public class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetRequest>
{
    public UpdateBudgetRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.BudgetType)
            .MaximumLength(32)
            .When(x => !string.IsNullOrEmpty(x.BudgetType));

        RuleFor(x => x.Status)
            .MaximumLength(16)
            .When(x => !string.IsNullOrEmpty(x.Status));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
