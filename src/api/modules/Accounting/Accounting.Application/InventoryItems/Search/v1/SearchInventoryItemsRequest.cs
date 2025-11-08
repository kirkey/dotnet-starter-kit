using Accounting.Application.InventoryItems.Responses;

namespace Accounting.Application.InventoryItems.Search.v1;

public sealed class SearchInventoryItemsRequest : PaginationFilter, IRequest<PagedList<InventoryItemResponse>>
{
    public string? Sku { get; init; }
    public string? Name { get; init; }
    public bool? IsActive { get; init; }
}

