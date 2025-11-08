namespace Accounting.Application.InventoryItems.Search.v1;

public sealed class SearchInventoryItemsSpec : Specification<InventoryItem>
{
    public SearchInventoryItemsSpec(SearchInventoryItemsRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!string.IsNullOrWhiteSpace(request.Sku))
        {
            Query.Where(i => i.Sku.Contains(request.Sku));
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            Query.Where(i => i.Name.Contains(request.Name));
        }

        if (request.IsActive.HasValue)
        {
            Query.Where(i => i.IsActive == request.IsActive.Value);
        }

        Query.Skip(request.PageNumber * request.PageSize)
             .Take(request.PageSize);

        Query.OrderBy(i => i.Sku);
    }
}

