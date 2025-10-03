using Store.Domain.Exceptions.StockLevel;

namespace FSH.Starter.WebApi.Store.Application.StockLevels.Get.v1;

/// <summary>
/// Handler for getting a stock level by ID.
/// </summary>
public sealed class GetStockLevelHandler(
    [FromKeyedServices("store:stocklevels")] IReadRepository<StockLevel> repository)
    : IRequestHandler<GetStockLevelCommand, StockLevelResponse>
{
    public async Task<StockLevelResponse> Handle(GetStockLevelCommand request, CancellationToken cancellationToken)
    {
        var stockLevel = await repository.FirstOrDefaultAsync(
            new Specs.GetStockLevelByIdSpec(request.Id),
            cancellationToken);

        if (stockLevel is null)
        {
            throw new StockLevelNotFoundException(request.Id);
        }

        return stockLevel;
    }
}
