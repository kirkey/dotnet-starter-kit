namespace Accounting.Application.InventoryItems.Deactivate.v1;

public sealed class DeactivateInventoryItemCommandValidator : AbstractValidator<DeactivateInventoryItemCommand>
{
    public DeactivateInventoryItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Inventory item ID is required.");
    }
}

