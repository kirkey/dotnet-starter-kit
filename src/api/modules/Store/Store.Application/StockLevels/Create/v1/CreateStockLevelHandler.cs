namespace FSH.Starter.WebApi.Store.Application.StockLevels.Create.v1;

/// <summary>
/// Handler for creating a stock level.
/// </summary>
public sealed class CreateStockLevelHandler(
    [FromKeyedServices("store:stocklevels")] IRepository<StockLevel> repository)
    : IRequestHandler<CreateStockLevelCommand, CreateStockLevelResponse>
{
    public async Task<CreateStockLevelResponse> Handle(CreateStockLevelCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = StockLevel.Create(
            request.ItemId,
            request.WarehouseId,
            request.WarehouseLocationId,
            request.BinId,
            request.LotNumberId,
            request.SerialNumberId,
            request.QuantityOnHand);

        await repository.AddAsync(stockLevel, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new CreateStockLevelResponse(stockLevel.Id);
    }
}
