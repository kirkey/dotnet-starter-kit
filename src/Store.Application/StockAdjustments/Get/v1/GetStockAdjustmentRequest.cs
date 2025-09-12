namespace FSH.Starter.WebApi.Store.Application.StockAdjustments.Get.v1;

public class GetStockAdjustmentRequest(DefaultIdType id) : IRequest<StockAdjustmentResponse>
{
    public DefaultIdType Id { get; set; } = id;
}
