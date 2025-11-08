namespace Accounting.Application.InventoryItems.Deactivate.v1;

public sealed record DeactivateInventoryItemCommand(DefaultIdType Id) : IRequest<DefaultIdType>;

