namespace FSH.Starter.WebApi.Store.Application.CycleCounts.Search.v1;

/// <summary>
/// Specification for searching cycle count items with item details.
/// </summary>
public class SearchCycleCountItemsSpec : Specification<CycleCountItem>
{
    public SearchCycleCountItemsSpec(SearchCycleCountItemsRequest request)
    {
        Query
            .Include(x => x.Item)
            .Include(x => x.CycleCount)
                .ThenInclude(x => x.WarehouseLocation);

        // Filter by cycle count
        if (request.CycleCountId.HasValue)
        {
            Query.Where(x => x.CycleCountId == request.CycleCountId.Value);
        }

        // Filter by item SKU
        if (!string.IsNullOrWhiteSpace(request.ItemSku))
        {
            Query.Where(x => x.Item.Sku != null && x.Item.Sku.Contains(request.ItemSku));
        }

        // Filter by item barcode
        if (!string.IsNullOrWhiteSpace(request.ItemBarcode))
        {
            Query.Where(x => x.Item.Barcode != null && x.Item.Barcode.Contains(request.ItemBarcode));
        }

        // Filter by item name
        if (!string.IsNullOrWhiteSpace(request.ItemName))
        {
            Query.Where(x => x.Item.Name != null && x.Item.Name.Contains(request.ItemName));
        }

        // Filter by counted status
        if (request.IsCounted.HasValue)
        {
            if (request.IsCounted.Value)
            {
                Query.Where(x => x.CountedQuantity != null);
            }
            else
            {
                Query.Where(x => x.CountedQuantity == null);
            }
        }

        // Filter by variance
        if (request.HasVariance.HasValue)
        {
            if (request.HasVariance.Value)
            {
                Query.Where(x => x.VarianceQuantity != null && x.VarianceQuantity != 0);
            }
            else
            {
                Query.Where(x => x.VarianceQuantity == null || x.VarianceQuantity == 0);
            }
        }

        // Filter by recount requirement
        if (request.RequiresRecount.HasValue)
        {
            Query.Where(x => x.RequiresRecount == request.RequiresRecount.Value);
        }

        // Search across multiple fields if keyword is provided
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            Query.Search(x => x.Item.Sku, $"%{request.Keyword}%")
                .Search(x => x.Item.Barcode, $"%{request.Keyword}%")
                .Search(x => x.Item.Name, $"%{request.Keyword}%");
        }
    }
}

