namespace Accounting.Application.ProjectCosting.Update.v1;

/// <summary>
/// Validator for UpdateProjectCostingCommand.
/// </summary>
public sealed class UpdateProjectCostingCommandValidator : AbstractValidator<UpdateProjectCostingCommand>
{
    public UpdateProjectCostingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Project costing entry ID is required.");

        RuleFor(x => x.EntryDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Entry date cannot be in the future.")
            .When(x => x.EntryDate.HasValue);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.")
            .When(x => x.Amount.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Category)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Category));

        RuleFor(x => x.CostCenter)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.CostCenter));

        RuleFor(x => x.WorkOrderNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.WorkOrderNumber));

        RuleFor(x => x.Vendor)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Vendor));

        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.InvoiceNumber));
    }
}
