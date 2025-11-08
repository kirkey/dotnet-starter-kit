using Accounting.Application.InventoryItems.Responses;

namespace Accounting.Application.InventoryItems.Get;

public sealed record GetInventoryItemRequest(DefaultIdType Id) : IRequest<InventoryItemResponse>;

