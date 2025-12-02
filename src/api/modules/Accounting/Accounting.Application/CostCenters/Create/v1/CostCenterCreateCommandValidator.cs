namespace Accounting.Application.CostCenters.Create.v1;

/// <summary>
/// Validator for cost center creation command.
/// </summary>
public class CostCenterCreateCommandValidator : AbstractValidator<CostCenterCreateCommand>
{
    public CostCenterCreateCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Cost center code is required")
            .MaximumLength(64).WithMessage("Cost center code cannot exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Cost center name is required")
            .MaximumLength(256).WithMessage("Cost center name cannot exceed 256 characters");

        RuleFor(x => x.CostCenterType)
            .NotEmpty().WithMessage("Cost center type is required")
            .Must(type => new[] { "Department", "Division", "BusinessUnit", "Project", "Location" }.Contains(type))
            .WithMessage("Cost center type must be one of: Department, Division, BusinessUnit, Project, Location");

        RuleFor(x => x.BudgetAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Budget amount cannot be negative");

        RuleFor(x => x.ManagerName)
            .MaximumLength(256).WithMessage("Manager name cannot exceed 256 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.ManagerName));

        RuleFor(x => x.Description)
            .MaximumLength(2048).WithMessage("Description cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Notes)
            .MaximumLength(2048).WithMessage("Notes cannot exceed 2048 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}

