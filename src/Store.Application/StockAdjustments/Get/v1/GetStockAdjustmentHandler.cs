

using Store.Domain.Exceptions.StockAdjustment;

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

public sealed class GetStockAdjustmentHandler(
    [FromKeyedServices("store:stock-adjustments")] IReadRepository<StockAdjustment> repository,
    ICacheService cache)
    : IRequestHandler<GetStockAdjustmentRequest, StockAdjustmentResponse>
{
    public async Task<StockAdjustmentResponse> Handle(GetStockAdjustmentRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"stock-adjustment:{request.Id}",
            async () =>
            {
                var spec = new GetStockAdjustmentSpecs(request.Id);
                var response = await repository.FirstOrDefaultAsync(spec, cancellationToken).ConfigureAwait(false) ?? 
                               throw new StockAdjustmentNotFoundException(request.Id);
                return response;
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        return item!;
    }
}
