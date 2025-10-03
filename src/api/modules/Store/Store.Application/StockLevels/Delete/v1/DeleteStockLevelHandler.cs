using Store.Domain.Exceptions.StockLevel;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Delete.v1;

/// <summary>
/// Handler for deleting a stock level record.
/// </summary>
public sealed class DeleteStockLevelHandler(
    [FromKeyedServices("store:stocklevels")] IRepository<StockLevel> repository,
    [FromKeyedServices("store:stocklevels")] IReadRepository<StockLevel> readRepository)
    : IRequestHandler<DeleteStockLevelCommand>
{
    public async Task Handle(DeleteStockLevelCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = await readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (stockLevel is null)
        {
            throw new StockLevelNotFoundException(request.Id);
        }

        // Business rule: Cannot delete stock level with positive quantities
        if (stockLevel.QuantityOnHand > 0)
        {
            throw new InvalidStockLevelOperationException(
                $"Cannot delete stock level with positive quantity on hand ({stockLevel.QuantityOnHand} units). Adjust quantity to zero first.");
        }

        await repository.DeleteAsync(stockLevel, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
