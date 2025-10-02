namespace Accounting.Application.Projects.Costing.Create;

/// <summary>
/// Validator for <see cref="CreateProjectCostingCommand"/>.
/// </summary>
public sealed class CreateProjectCostingCommandValidator : AbstractValidator<CreateProjectCostingCommand>
{
    public CreateProjectCostingCommandValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required.");

        RuleFor(x => x.EntryDate)
            .NotEmpty()
            .WithMessage("Entry date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Entry date cannot be in the future.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Cost amount must be positive.")
            .LessThanOrEqualTo(1_000_000_000m)
            .WithMessage("Cost amount is too large.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Cost description is required.")
            .Length(2, 500)
            .WithMessage("Cost description must be between 2 and 500 characters.");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

        RuleFor(x => x.Category)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.Category))
            .WithMessage("Category cannot exceed 100 characters.");

        RuleFor(x => x.CostCenter)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.CostCenter))
            .WithMessage("Cost center cannot exceed 50 characters.");

        RuleFor(x => x.WorkOrderNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.WorkOrderNumber))
            .WithMessage("Work order number cannot exceed 50 characters.");

        RuleFor(x => x.Vendor)
            .MaximumLength(200)
            .When(x => !string.IsNullOrWhiteSpace(x.Vendor))
            .WithMessage("Vendor name cannot exceed 200 characters.");

        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.InvoiceNumber))
            .WithMessage("Invoice number cannot exceed 50 characters.");
    }
}
