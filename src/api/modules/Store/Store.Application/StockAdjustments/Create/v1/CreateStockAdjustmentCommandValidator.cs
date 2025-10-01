namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Create.v1;

public class CreateStockAdjustmentCommandValidator : AbstractValidator<CreateStockAdjustmentCommand>
{
    private static readonly string[] AllowedAdjustmentTypes = new[] { "Physical Count", "Damage", "Loss", "Found", "Transfer", "Other", "Increase", "Decrease", "Write-Off" };

    public CreateStockAdjustmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Adjustment name is required")
            .MaximumLength(100)
            .WithMessage("Adjustment name must not exceed 100 characters");

        RuleFor(x => x.AdjustmentNumber)
            .NotEmpty()
            .WithMessage("Adjustment number is required")
            .MaximumLength(50)
            .WithMessage("Adjustment number must not exceed 50 characters");

        RuleFor(x => x.GroceryItemId)
            .NotEmpty()
            .WithMessage("Grocery item ID is required");

        RuleFor(x => x.WarehouseId)
            .NotEmpty()
            .WithMessage("Warehouse ID is required");

        RuleFor(x => x.WarehouseLocationId)
            .NotEmpty()
            .When(x => x.WarehouseLocationId.HasValue)
            .WithMessage("Warehouse location ID, when provided, must not be empty");

        RuleFor(x => x.AdjustmentDate)
            .NotEmpty()
            .WithMessage("Adjustment date is required")
            .LessThanOrEqualTo(DateTime.Today.AddDays(1))
            .WithMessage("Adjustment date cannot be in the future");

        RuleFor(x => x.AdjustmentType)
            .NotEmpty()
            .WithMessage("Adjustment type is required")
            .Must(type => AllowedAdjustmentTypes.Contains(type))
            .WithMessage($"Adjustment type must be one of: {string.Join(", ", AllowedAdjustmentTypes)}");

        RuleFor(x => x.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(status => new[] { "Pending", "Approved", "Completed", "Cancelled" }.Contains(status))
            .WithMessage("Status must be one of: Pending, Approved, Completed, Cancelled");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Adjustment reason is required")
            .MaximumLength(200)
            .WithMessage("Adjustment reason must not exceed 200 characters");

        RuleFor(x => x.QuantityBefore)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity before must be zero or greater");

        RuleFor(x => x.AdjustmentQuantity)
            .GreaterThan(0)
            .WithMessage("Adjustment quantity must be greater than zero");

        RuleFor(x => x.UnitCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Unit cost must be zero or greater");

        RuleFor(x => x.Reference)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Reference));

        RuleFor(x => x.AdjustedBy)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.AdjustedBy));

        RuleFor(x => x.BatchNumber)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.BatchNumber));

        RuleFor(x => x.ExpiryDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiryDate.HasValue)
            .WithMessage("Expiry date must be in the future");
    }
}
