namespace Accounting.Application.Projects.Update.v1;

/// <summary>
/// Validator for the UpdateProjectCommand with comprehensive business rule validation.
/// </summary>
public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project ID is required.");

        RuleFor(x => x.Name)
            .Length(2, 200)
            .WithMessage("Project name must be between 2 and 200 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-_\.]+$")
            .WithMessage("Project name can only contain letters, numbers, spaces, hyphens, underscores, and periods.")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(30))
            .WithMessage("Project start date cannot be more than 30 days in the future.")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("Project end date must be after start date.")
            .When(x => x.EndDate.HasValue && x.StartDate.HasValue);

        RuleFor(x => x.BudgetedAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Project budget amount must be non-negative.")
            .LessThanOrEqualTo(decimal.MaxValue)
            .WithMessage("Project budget amount is too large.")
            .When(x => x.BudgetedAmount.HasValue);

        RuleFor(x => x.Status)
            .Must(status => new[] { "Active", "Completed", "On Hold", "Cancelled" }.Contains(status))
            .WithMessage("Project status must be one of: Active, Completed, On Hold, Cancelled.")
            .When(x => !string.IsNullOrWhiteSpace(x.Status));

        RuleFor(x => x.ClientName)
            .Length(2, 100)
            .WithMessage("Client name must be between 2 and 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ClientName));

        RuleFor(x => x.ProjectManager)
            .Length(2, 100)
            .WithMessage("Project manager name must be between 2 and 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.ProjectManager));

        RuleFor(x => x.Department)
            .Length(2, 50)
            .WithMessage("Department name must be between 2 and 50 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Department));

        RuleFor(x => x.Description)
            .MaximumLength(1024)
            .WithMessage("Project description cannot exceed 1000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048)
            .WithMessage("Project notes cannot exceed 2000 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));

        // Business rule: Cannot set status to Completed without EndDate
        RuleFor(x => x)
            .Must(x => x.EndDate.HasValue)
            .WithMessage("End date is required when marking project as Completed.")
            .When(x => x.Status == "Completed");

        // Business rule: Cannot set status to Cancelled without EndDate
        RuleFor(x => x)
            .Must(x => x.EndDate.HasValue)
            .WithMessage("End date is required when marking project as Cancelled.")
            .When(x => x.Status == "Cancelled");
    }
}
