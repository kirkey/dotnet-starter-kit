namespace Accounting.Application.Projects.Create;

public class CreateProjectRequestValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.BudgetedAmount)
            .GreaterThan(0);

        RuleFor(x => x.ClientName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.ClientName));

        RuleFor(x => x.ProjectManager)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.ProjectManager));

        RuleFor(x => x.Department)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Department));

        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
