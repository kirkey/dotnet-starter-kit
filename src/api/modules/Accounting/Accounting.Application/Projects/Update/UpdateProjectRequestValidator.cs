namespace Accounting.Application.Projects.Update;

public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.BudgetedAmount)
            .GreaterThan(0)
            .When(x => x.BudgetedAmount.HasValue);

        RuleFor(x => x.Status)
            .MaximumLength(16)
            .When(x => !string.IsNullOrEmpty(x.Status));

        RuleFor(x => x.ClientName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.ClientName));

        RuleFor(x => x.ProjectManager)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.ProjectManager));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
