

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

public class GetStockAdjustmentSpecs : Specification<StockAdjustment, StockAdjustmentResponse>
{
    public GetStockAdjustmentSpecs(DefaultIdType id)
    {
        Query
            .Where(s => s.Id == id);
    }
}
