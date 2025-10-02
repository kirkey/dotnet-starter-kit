namespace Accounting.Application.ProjectCosting.Create.v1;

/// <summary>
/// Validator for CreateProjectCostingCommand.
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
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Entry date cannot be in the future.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("Account ID is required.");

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

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
