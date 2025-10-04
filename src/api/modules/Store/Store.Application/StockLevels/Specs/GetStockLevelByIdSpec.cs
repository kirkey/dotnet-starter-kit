using FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Specs;

/// <summary>
/// Specification to get a stock level by ID with response mapping.
/// </summary>
public sealed class GetStockLevelByIdSpec : Specification<StockLevel, StockLevelResponse>
{
    public GetStockLevelByIdSpec(DefaultIdType id)
    {
        Query.Where(s => s.Id == id);
    }
}
