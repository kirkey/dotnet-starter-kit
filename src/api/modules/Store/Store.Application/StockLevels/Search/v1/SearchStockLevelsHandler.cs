namespace FSH.Starter.WebApi.Store.Application.StockLevels.Search.v1;

/// <summary>
/// Handler for searching stock levels.
/// </summary>
public sealed class SearchStockLevelsHandler(
    [FromKeyedServices("store:stocklevels")] IReadRepository<StockLevel> repository)
    : IRequestHandler<SearchStockLevelsCommand, PagedList<StockLevelResponse>>
{
    public async Task<PagedList<StockLevelResponse>> Handle(SearchStockLevelsCommand request, CancellationToken cancellationToken)
    {
        var spec = new Specs.SearchStockLevelsSpec(request);

        var stockLevels = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        var stockLevelResponses = stockLevels.Select(stockLevel => new StockLevelResponse
        {
            Id = stockLevel.Id,
            ItemId = stockLevel.ItemId,
            WarehouseId = stockLevel.WarehouseId,
            QuantityOnHand = stockLevel.QuantityOnHand,
            QuantityReserved = stockLevel.QuantityReserved,
            QuantityOnOrder = stockLevel.QuantityOnOrder,
            ReorderPoint = stockLevel.ReorderPoint,
            MaximumLevel = stockLevel.MaximumLevel,
            UnitOfMeasure = stockLevel.UnitOfMeasure,
            LastUpdated = stockLevel.LastModifiedOn ?? stockLevel.CreatedOn
        }).ToList();

        return new PagedList<StockLevelResponse>(stockLevelResponses, request.PageNumber, request.PageSize, totalCount);
    }
}
