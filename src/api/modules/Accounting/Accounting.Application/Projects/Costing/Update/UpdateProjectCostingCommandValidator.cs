namespace Accounting.Application.Projects.Costing.Update;

/// <summary>
/// Validator for <see cref="UpdateProjectCostingCommand"/>.
/// </summary>
public sealed class UpdateProjectCostingCommandValidator : AbstractValidator<UpdateProjectCostingCommand>
{
    public UpdateProjectCostingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project costing entry ID is required.");

        RuleFor(x => x.EntryDate)
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.EntryDate.HasValue)
            .WithMessage("Entry date cannot be in the future.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .When(x => x.Amount.HasValue)
            .WithMessage("Cost amount must be positive.")
            .LessThanOrEqualTo(1_000_000_000m)
            .When(x => x.Amount.HasValue)
            .WithMessage("Cost amount is too large.");

        RuleFor(x => x.Description)
            .Length(2, 500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Cost description must be between 2 and 500 characters.");

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
