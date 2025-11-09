namespace Accounting.Application.InventoryItems.ReduceStock.v1;

public sealed class ReduceStockCommandValidator : AbstractValidator<ReduceStockCommand>
{
    public ReduceStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory item ID is required.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Quantity must not exceed 999,999,999.99.");
    }
}

