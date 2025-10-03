using Store.Domain.Exceptions.StockLevel;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Allocate.v1;

/// <summary>
/// Handler for allocating reserved stock to pick lists.
/// </summary>
public sealed class AllocateStockHandler(
    [FromKeyedServices("store:stocklevels")] IRepository<StockLevel> repository,
    [FromKeyedServices("store:stocklevels")] IReadRepository<StockLevel> readRepository)
    : IRequestHandler<AllocateStockCommand, AllocateStockResponse>
{
    public async Task<AllocateStockResponse> Handle(AllocateStockCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = await readRepository.GetByIdAsync(request.StockLevelId, cancellationToken);

        if (stockLevel is null)
        {
            throw new StockLevelNotFoundException(request.StockLevelId);
        }

        // Allocate the quantity (domain method handles business rules)
        stockLevel.AllocateQuantity(request.Quantity);

        await repository.UpdateAsync(stockLevel, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return new AllocateStockResponse(
            stockLevel.Id,
            request.Quantity,
            stockLevel.QuantityReserved);
    }
}
