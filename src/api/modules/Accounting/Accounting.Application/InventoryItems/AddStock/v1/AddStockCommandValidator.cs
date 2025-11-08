namespace Accounting.Application.InventoryItems.AddStock.v1;

public sealed class AddStockCommandValidator : AbstractValidator<AddStockCommand>
{
    public AddStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory item ID is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be positive.")
            .LessThanOrEqualTo(999999999.99m).WithMessage("Amount must not exceed 999,999,999.99.");
    }
}

