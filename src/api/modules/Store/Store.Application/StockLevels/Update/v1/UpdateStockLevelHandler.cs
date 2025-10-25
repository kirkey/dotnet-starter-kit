using Store.Domain.Exceptions.StockLevel;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Update.v1;

/// <summary>
/// Handler for updating a stock level record.
/// Note: This handler updates location/bin/lot/serial assignments only.
/// Use specific commands (Reserve, Allocate, etc.) for quantity operations.
/// </summary>
public sealed class UpdateStockLevelHandler(
    [FromKeyedServices("store:stocklevels")] IRepository<StockLevel> repository,
    [FromKeyedServices("store:stocklevels")] IReadRepository<StockLevel> readRepository)
    : IRequestHandler<UpdateStockLevelCommand, UpdateStockLevelResponse>
{
    public async Task<UpdateStockLevelResponse> Handle(UpdateStockLevelCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = await readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (stockLevel is null)
        {
            throw new StockLevelNotFoundException(request.Id);
        }

        // Update location/bin/lot/serial assignments
        stockLevel.UpdateLocationAssignments(
            request.WarehouseLocationId,
            request.BinId,
            request.LotNumberId,
            request.SerialNumberId);
        
        await repository.UpdateAsync(stockLevel, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new UpdateStockLevelResponse(stockLevel.Id);
    }
}
