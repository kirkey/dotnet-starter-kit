namespace FSH.Starter.WebApi.Store.Application.InventoryTransfers.Items.Add.v1;

public class AddInventoryTransferItemCommandValidator : AbstractValidator<AddInventoryTransferItemCommand>
{
    public AddInventoryTransferItemCommandValidator()
    {
        RuleFor(x => x.InventoryTransferId).NotEmpty();
        RuleFor(x => x.GroceryItemId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
        RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative");
    }
}

