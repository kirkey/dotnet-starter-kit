namespace Accounting.Application.Projects.Create.v1;

/// <summary>
/// Validator for the CreateProjectCommand with comprehensive business rule validation.
/// </summary>
public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Project name is required.")
            .Length(2, 200)
            .WithMessage("Project name must be between 2 and 200 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-_\.]+$")
            .WithMessage("Project name can only contain letters, numbers, spaces, hyphens, underscores, and periods.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Project start date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30))
            .WithMessage("Project start date cannot be more than 30 days in the future.");

        RuleFor(x => x.BudgetedAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Project budget amount must be non-negative.")
            .LessThanOrEqualTo(decimal.MaxValue)
            .WithMessage("Project budget amount is too large.");

        RuleFor(x => x.ClientName)
            .Length(2, 100)
            .WithMessage("Client name must be between 2 and 100 characters when provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.ClientName));

        RuleFor(x => x.ProjectManager)
            .Length(2, 100)
            .WithMessage("Project manager name must be between 2 and 100 characters when provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProjectManager));

        RuleFor(x => x.Department)
            .Length(2, 50)
            .WithMessage("Department name must be between 2 and 50 characters when provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.Description)
            .MaximumLength(1024)
            .WithMessage("Project description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Project notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
