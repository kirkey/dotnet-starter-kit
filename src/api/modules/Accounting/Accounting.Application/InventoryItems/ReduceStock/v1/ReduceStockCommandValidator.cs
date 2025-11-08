namespace Accounting.Application.InventoryItems.ReduceStock.v1;

public sealed class ReduceStockCommandValidator : AbstractValidator<ReduceStockCommand>
{
    public ReduceStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory item ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
    }
}

