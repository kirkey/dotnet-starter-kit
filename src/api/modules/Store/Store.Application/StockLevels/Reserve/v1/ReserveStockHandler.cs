using Store.Domain.Exceptions.StockLevel;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Reserve.v1;

/// <summary>
/// Handler for reserving stock quantity.
/// </summary>
public sealed class ReserveStockHandler(
    [FromKeyedServices("store:stocklevels")] IRepository<StockLevel> repository,
    [FromKeyedServices("store:stocklevels")] IReadRepository<StockLevel> readRepository)
    : IRequestHandler<ReserveStockCommand, ReserveStockResponse>
{
    public async Task<ReserveStockResponse> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = await readRepository.GetByIdAsync(request.StockLevelId, cancellationToken);

        if (stockLevel is null)
        {
            throw new StockLevelNotFoundException(request.StockLevelId);
        }

        // Reserve the quantity (domain method handles business rules)
        stockLevel.ReserveQuantity(request.Quantity);

        await repository.UpdateAsync(stockLevel, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new ReserveStockResponse(
            stockLevel.Id,
            request.Quantity,
            stockLevel.QuantityAvailable);
    }
}
