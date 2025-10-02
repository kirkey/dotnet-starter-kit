namespace Accounting.Application.Projects.Costing.Create.v1;

/// <summary>
/// Validator for the CreateProjectCostCommand with comprehensive business rule validation.
/// </summary>
public class CreateProjectCostCommandValidator : AbstractValidator<CreateProjectCostCommand>
{
    public CreateProjectCostCommandValidator()
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
            .LessThanOrEqualTo(decimal.MaxValue)
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
            .Length(1, 50)
            .WithMessage("Cost category must be between 1 and 50 characters when provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.Category));

        RuleFor(x => x.CostCenter)
            .Length(1, 20)
            .WithMessage("Cost center must be between 1 and 20 characters when provided.")
            .Matches(@"^[A-Z0-9\-]+$")
            .WithMessage("Cost center can only contain uppercase letters, numbers, and hyphens.")
            .When(x => !string.IsNullOrWhiteSpace(x.CostCenter));

        RuleFor(x => x.WorkOrderNumber)
            .Length(1, 50)
            .WithMessage("Work order number must be between 1 and 50 characters when provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.WorkOrderNumber));

        RuleFor(x => x.Vendor)
            .Length(1, 100)
            .WithMessage("Vendor name must be between 1 and 100 characters when provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.Vendor));

        RuleFor(x => x.InvoiceNumber)
            .Length(1, 50)
            .WithMessage("Invoice number must be between 1 and 50 characters when provided.")
            .When(x => !string.IsNullOrWhiteSpace(x.InvoiceNumber));
    }
}
