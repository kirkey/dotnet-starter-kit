

namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

public class GetStockAdjustmentSpecification : Specification<StockAdjustment>
{
    public GetStockAdjustmentSpecification(DefaultIdType id)
    {
        Query.Where(sa => sa.Id == id);
        Query.Include(sa => sa.Warehouse);
    }
}
